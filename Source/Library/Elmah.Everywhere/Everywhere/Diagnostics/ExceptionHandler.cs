﻿using System;
using System.Linq;
using System.Collections.Generic;
using Elmah.Everywhere.Appenders;
using System.Reflection;
using Elmah.Everywhere.Utils;

#if !SILVERLIGHT
using Elmah.Everywhere.Configuration;
using System.Globalization;
using System.Reflection;

#endif


namespace Elmah.Everywhere.Diagnostics
{
    public sealed class ExceptionHandler
    {
        private static ExceptionWritterBase _writter;
        private static ExceptionDefaults _parameters;
        private static IEnumerable<Type> _appenders;

        static ExceptionHandler()
        {
            IsEnabled = true;
        }

        private ExceptionHandler()
        {
        }

        #if !SILVERLIGHT

        public static void ConfigureFromConfigurationFile(ExceptionWritterBase writter, IEnumerable<Type> appenders)
        {
            var configuration = (EverywhereConfigurationSection)System.Configuration.ConfigurationManager.GetSection(EverywhereConfigurationSection.SectionKey);
            if (configuration == null)
            {
                // If section is not configured use build in section.
                configuration = GetBuildInSection();
            }
            Configure(writter, configuration, appenders);
        }

        public static void Configure(ExceptionWritterBase writter, EverywhereConfigurationSection section, IEnumerable<Type> appenders)
        {
            if (section == null)
            {
                throw new ArgumentNullException("section");
            }
            var parameters = new ExceptionDefaults
            {
                ApplicationName = section.ApplicationName,
                Host = section.Host,
                Token = section.Token,
                RemoteLogUri = new Uri(section.RemoteLogUri)
            };
            Configure(writter, parameters, appenders);
        }
#endif

        public static void Configure(ExceptionWritterBase writter, ExceptionDefaults parameters, IEnumerable<Type> appenders)
        {
            if (writter == null)
            {
                throw new ArgumentNullException("writter");
            }
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            ValidateParameters(parameters);
            _appenders = appenders;
            if (_appenders == null)
            {
                _appenders = AllAppenders();
            }
            var httpWriter = writter as HttpExceptionWritterBase;
            if (httpWriter != null && httpWriter.RequestUri == null)
            {
                httpWriter.RequestUri = parameters.RemoteLogUri;
            }
            _writter = writter;
            _parameters = parameters;
            _writter.Completed += Writter_Completed;
        }

        private static void ValidateParameters(ExceptionDefaults parameters)
        {
            if (parameters.RemoteLogUri == null)
            {
                throw new ArgumentNullException("parameters", "RemoteLogUri is required");
            }
            if (string.IsNullOrWhiteSpace(parameters.ApplicationName))
            {
                parameters.ApplicationName = "NOT-SET";
            }
            if (string.IsNullOrWhiteSpace(parameters.Host))
            {
                parameters.Host = "NOT_SET";
            }
        }

        public static ErrorInfo Report(Exception exception)
        {
            return Report(exception, null);
        }

        public static ErrorInfo Report(Exception exception, IDictionary<string, object> propeties)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }
            ErrorInfo error = null;
            if (IsEnabled)
            {
                error = new ErrorInfo(exception)
                {
                    ApplicationName = _parameters.ApplicationName,
                    Host = _parameters.Host,
                    Appenders = _appenders,
                    Properties = propeties
                };
                error.EnsureAppenders();
                _writter.Write(_parameters.Token, error);
            }
            return error;
        }

        public static IEnumerable<Type> AllAppenders()
        {
            var types = Utility.GetTypes(typeof (ExceptionHandler).Assembly, Utility.IsBaseAppenderAssignableType);
            return types;
        }

#if !SILVERLIGHT

        public static void Attach(AppDomain domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }

            // Fires on any unhandled exception on any threrad. Forces application to shut down after event handler completes.

            // To prevent shutdown use this configuration.

            //<configuration>
            //    <runtime>
            //        <legacyUnhandledExceptionPolicy enabled="1" />
            //    </runtime>
            //</configuration>

            domain.UnhandledException += AppDomain_UnhandledException;
        }

        public static void Detach(AppDomain domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            domain.UnhandledException -= AppDomain_UnhandledException;
        }

#endif

        #region Private methods

        private static void Writter_Completed(object sender, WritterEventArgs e)
        {
            var writter = sender as ExceptionWritterBase;
            if (writter == null)
            {
                return;
            }
#if !SILVERLIGHT
            System.Diagnostics.Trace.WriteLine(e.Error.BuildMessage());
            if (writter.Exception != null)
            {
                System.Diagnostics.Trace.WriteLine(writter.Exception.ToString());
            }
#endif
        }

        private static void AppDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Report((Exception) args.ExceptionObject, null);
        }

#if !SILVERLIGHT

        private static EverywhereConfigurationSection GetBuildInSection()
        {
            var section = new EverywhereConfigurationSection();
            section.ApplicationName = "Default-Handler";
            section.Host = "Default-Handler";
            section.Token = "Default-Handler";
            section.RemoteLogUri = "http://localhost:11079/error/log";
            return section;
        }

#endif

        #endregion

        public static bool IsEnabled { get; set; }
    }
}