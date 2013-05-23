using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Commands
{
    public interface ICommandHandlerFactory
    {
        ICommandHandler<TCommand>[] GetHandlers<TCommand>() where TCommand : ICommand;

        ICommandHandler<TCommand, TResult>[] GetHandlers<TCommand, TResult>() where TCommand : ICommand;

        void ReleaseHandler(object releasingHandler);
    }
}
