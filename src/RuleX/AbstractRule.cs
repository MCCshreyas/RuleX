﻿using System;

namespace RuleX
{
    /// <summary>
    /// Abstract implementation of Rule.
    /// Rules must derived from this class and implement <see cref="Execute"/> method.
    /// </summary>
    public abstract class AbstractRule
    {
        /// <summary>
        /// Get's called before executing actual Rule implementation
        /// </summary>
        /// <returns>Boolean that represent should rule executed or not</returns>
        public virtual bool BeforeExecute() => true;

        /// <summary>
        /// Get's called after executing rule.
        /// </summary>
        /// <param name="context">contains the rule execution result and other connected factors.</param>
        public virtual void AfterExecute(RuleExecutedContext context)
        {
        }

        /// <summary>
        /// Abstract method that should be implemented by deriving class.
        /// </summary>
        /// <remarks>This will contain actual rule business logic.</remarks>
        /// <returns></returns>
        public abstract RuleResult Execute();

        internal RuleResult Process()
        {
            var result = RuleResult.Default();
            var skipped = BeforeExecute();

            var executedContext = new RuleExecutedContext(false, RuleResult.Default());

            if (skipped)
            {
                executedContext = new RuleExecutedContext(false,RuleResult.Default());
                try
                {
                    result = Execute();
                }
                catch (Exception e)
                {
                    executedContext = new RuleExecutedContext(false, RuleResult.Default());
                    executedContext.SetException(e);
                }
            }
            AfterExecute(executedContext);
            return result;
        }
    }
}
