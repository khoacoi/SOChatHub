using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Common.Commands
{
    public class CommandProcessor : ICommandProcessor
    {
        private readonly ICommandHandlerFactory _factory;

        public CommandProcessor(ICommandHandlerFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Processes the specified command.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command.</param>
        /// <exception cref="CommandHandlerNotFoundException"></exception>
        public void Process<TCommand>(TCommand command) where TCommand : ICommand
        {
            Validator.ValidateObject(command, new ValidationContext(command, null, null), true);

            //var handlers = ServiceLocator.Current.GetAllInstances<ICommandHandler<TCommand>>();
            var handlers = _factory.GetHandlers<TCommand>();
            if (handlers == null || !handlers.Any())
            {
                throw new CommandHandlerNotFoundException(typeof(TCommand));
            }

            foreach (var handler in handlers)
            {
                try
                {
                    handler.Handle(command);
                }
                finally
                {
                    _factory.ReleaseHandler(handler);
                }
            }
        }

        /// <summary>
        /// Processes the specified command.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        /// <exception cref="CommandHandlerNotFoundException"></exception>
        public IEnumerable<TResult> Process<TCommand, TResult>(TCommand command) where TCommand : ICommand
        {
            Validator.ValidateObject(command, new ValidationContext(command, null, null), true);

            //var handlers = ServiceLocator.Current.GetAllInstances<ICommandHandler<TCommand, TResult>>();
            var handlers = _factory.GetHandlers<TCommand, TResult>();
            if (handlers == null || !handlers.Any())
            {
                throw new CommandHandlerNotFoundException(typeof(TCommand), typeof(TResult));
            }

            foreach (var handler in handlers)
            {
                TResult result;
                try
                {
                    result = handler.Handle(command);
                }
                finally
                {
                    _factory.ReleaseHandler(handler);
                }

                yield return result;
            }
        }

        /// <summary>
        /// Processes the specified command.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="command">The command.</param>
        /// <param name="resultHandler">The result handler.</param>
        public void Process<TCommand, TResult>(TCommand command, Action<TResult> resultHandler) where TCommand : ICommand
        {
            foreach (var result in Process<TCommand, TResult>(command))
            {
                resultHandler(result);
            }
        }
    }
}
