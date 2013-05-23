using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Common.Data;

namespace App.Common.Commands
{
    public abstract class CommandHandlerBase<TCommand, TResult> : ICommandHandler<TCommand, TResult>
        where TCommand : ICommand
    {
        public IRepositoryFactory RepositoryFactory { get; set; }

        public abstract TResult Handle(TCommand command);
    }

    public abstract class CommandHandlerBase<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public IRepositoryFactory RepositoryFactory { get; set; }

        public abstract void Handle(TCommand command);
    }
}
