using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Builder;

namespace Xunit.Autofac
{
    public static class AutofacRegistrationExtensions
    {
        /// <summary>
        /// Configure the component so that every dependent component or call to Resolve() within
        /// a ILifetimeScope assotiated with a test collection gets the same, shared instance.
        /// Dependent components in lifetime scopes that are children of the tagged scope will
        /// share the parent's instance.
        /// </summary>
        /// <returns>A registration builder allowing further configuration of the component.</returns>
        public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle>
            InstancePerTestCollection<TLimit, TActivatorData, TRegistrationStyle>(
                this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder)
        {
            return builder.InstancePerMatchingLifetimeScope(LifetimeScopeTags.TestCollection);
        }
    }
}
