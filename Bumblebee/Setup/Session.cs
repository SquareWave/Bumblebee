using System;
using System.Collections.Generic;
using System.Linq;
using Bumblebee.Interfaces;
using OpenQA.Selenium;

namespace Bumblebee.Setup
{
    public class Session
    {
        [ThreadStatic] public static Session Current;

        public virtual IWebDriver Driver { get; protected set; }

        public IMonkey Monkey { get; protected set; }

        public Session(IDriverEnvironment environment)
        {
            Driver = environment.CreateWebDriver();
            Current = this;
        }

        public TBlock NavigateTo<TBlock>(string url) where TBlock : IBlock
        {
            Driver.Navigate().GoToUrl(url);
            return CurrentBlock<TBlock>();
        }

        public TBlock CurrentBlock<TBlock>(IWebElement tag = null) where TBlock : IBlock
        {
            var type = typeof(TBlock);
            IList<Type> constructorSignature = new List<Type> { typeof(Session) };
            IList<object> constructorArgs = new List<object> { this };

            if (typeof(ISpecificBlock).IsAssignableFrom(typeof(TBlock)))
            {
                constructorSignature.Add(typeof(IWebElement));
                constructorArgs.Add(tag);
            }

            var constructor = type.GetConstructor(constructorSignature.ToArray());

            if (constructor == null)
            {
                throw new ArgumentException("The result type specified (" + type + ") is not a valid block. " +
                                            "It must have a constructor that takes only a session.");
            }

            return (TBlock) constructor.Invoke(constructorArgs.ToArray());
        }

        public virtual void End()
        {
            Driver.Quit();
        }
    }
}
