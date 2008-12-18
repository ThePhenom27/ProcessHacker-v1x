﻿/*
 * Process Hacker
 * 
 * Copyright (C) 2008 Dean
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;

namespace ProcessHacker.FormHelper
{
    /// <summary>
    /// Exception thrown when an
    /// operation is already in progress.
    /// </summary>
    public class AlreadyRunningException : System.ApplicationException
    {
        public AlreadyRunningException() : base("Operation already running")
        { }
    }

    public abstract class AsyncOperation
    {       
        public AsyncOperation(ISynchronizeInvoke target)
        {
            isiTarget = target;
            isRunning = false;
        }
       
        public void Start()
        {
            lock (this)
            {
                if (isRunning)
                {
                    throw new AlreadyRunningException();
                }
                isRunning = true;
            }
            new MethodInvoker(InternalStart).BeginInvoke(null, null);
        }        
        public void Cancel()
        {
            lock (this)
            {
                cancelledFlag = true;
            }
        }

        public bool CancelAndWait()
        {
            lock (this)
            {               
                cancelledFlag = true;

                while (!IsDone)
                {
                    Monitor.Wait(this, 1000);
                }
            }
            return !HasCompleted;
        }

        public bool WaitUntilDone()
        {
            lock (this)
            {
                // Wait for either completion or cancellation.  As with
                // CancelAndWait, we don't sleep forever - to reduce the
                // chances of deadlock in obscure race conditions, we wake
                // up every second to check we didn't miss a Pulse.
                while (!IsDone)
                {
                    Monitor.Wait(this, 1000);
                }
            }
            return HasCompleted;
        }
        public bool IsDone
        {
            get
            {
                lock (this)
                {
                    return completeFlag || cancelAcknowledgedFlag || failedFlag;
                }
            }
        }        
        public event EventHandler Completed;              
        public event EventHandler Cancelled;       
        public event System.Threading.ThreadExceptionEventHandler Failed;     
        protected ISynchronizeInvoke Target
        {
            get { return isiTarget; }
        }
        private ISynchronizeInvoke isiTarget;

        /// <summary>
        /// To be overridden by the deriving class
        /// </summary>
        protected abstract void DoWork();
        
        protected bool CancelRequested
        {
            get
            {
                lock (this) { return cancelledFlag; }
            }
        }
        private bool cancelledFlag;
      
        protected bool HasCompleted
        {
            get
            {
                lock (this) { return completeFlag; }
            }
        }
        private bool completeFlag;
        
      
        protected void AcknowledgeCancel()
        {
            lock (this)
            {
                cancelAcknowledgedFlag = true;
                isRunning = false;
                Monitor.Pulse(this);
                FireAsync(Cancelled, this, EventArgs.Empty);
            }
        }
        private bool cancelAcknowledgedFlag;
        // if the operation fails with an exception,set to true
        private bool failedFlag;
        // if the operation is running,set to true
        private bool isRunning;
                
        private void InternalStart()
        {            
            cancelledFlag = false;
            completeFlag = false;
            cancelAcknowledgedFlag = false;
            failedFlag = false;           
            try
            {
                DoWork();
            }
            catch (Exception e)
            {                
                try
                {
                    FailOperation(e);
                }
                catch
                { }               
                if (e is SystemException)
                {
                    throw;
                }
            }
            lock (this)
            {
                // raise the Completion event 
                if (!cancelAcknowledgedFlag && !failedFlag)
                {
                    CompleteOperation();
                }
            }
        } 
        private void CompleteOperation()
        {
            lock (this)
            {
                completeFlag = true;
                isRunning = false;
                Monitor.Pulse(this);                
                FireAsync(Completed, this, EventArgs.Empty);
            }
        }
        private void FailOperation(Exception e)
        {
            lock (this)
            {
                failedFlag = true;
                isRunning = false;
                Monitor.Pulse(this);
                FireAsync(Failed, this, new ThreadExceptionEventArgs(e));
            }
        }
        protected void FireAsync(Delegate dlg, params object[] pList)
        {
            if (dlg != null)
            {
                Target.BeginInvoke(dlg, pList);
            }
        }
    }
}

