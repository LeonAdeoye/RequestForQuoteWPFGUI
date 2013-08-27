﻿using System;
using System.Windows.Input;

namespace RequestForQuoteInterfacesLibrary.Commands
{
    public abstract class AbstractRequestForQuoteCommand : ICommand
    {
        protected AbstractRequestForQuoteCommand(){}

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);
    }
}