namespace Innofactor.XTB.AutoDeployer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;

    internal class Resource
    {
        #region Public Constructors

        public Resource(Func<QueryExpression, IEnumerable<Entity>> retrieveMultiple, string location)
        {
            Location = location;
            Destination = Classify(location);

            switch (Destination)
            {
                case Destination.Backend:
                    Target = Execute(retrieveMultiple, GetPluginAssemblyQuery(Location));
                    break;

                case Destination.Frontend:
                    Target = Execute(retrieveMultiple, GetWebResourceQuery(Location));
                    break;
            }
        }

        #endregion Public Constructors

        #region Internal Properties

        internal Destination Destination
        {
            get;
        }

        internal DateTime LastWriteTime
        {
            get;
            set;
        } = new DateTime(0);

        internal string Location
        {
            get;
        }

        internal EntityReference Target
        {
            get;
        }

        #endregion Internal Properties

        #region Internal Methods

        internal static byte[] GetContents(string location)
        {
            byte[] buffer = null;
            using (var stream = new FileStream(location, FileMode.Open, FileAccess.Read))
            {
                buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int)stream.Length);
            }

            return buffer;
        }

        #endregion Internal Methods

        #region Private Methods

        private static QueryExpression GetPluginAssemblyQuery(string location)
        {
            var assembly = Assembly.Load(GetContents(location));

            var chunks = assembly.FullName.Split(new string[] { ", ", "Version=", "Culture=", "PublicKeyToken=" }, StringSplitOptions.RemoveEmptyEntries);

            var query = new QueryExpression("pluginassembly");
            query.Criteria.AddCondition("name", ConditionOperator.Equal, chunks[0]);
            query.Criteria.AddCondition("version", ConditionOperator.Equal, chunks[1]);
            query.Criteria.AddCondition("culture", ConditionOperator.Equal, chunks[2]);
            query.Criteria.AddCondition("publickeytoken", ConditionOperator.Equal, chunks[3]);

            return query;
        }

        private static QueryExpression GetWebResourceQuery(string location)
        {
            var query = new QueryExpression("webresource");
            query.Criteria.AddCondition("name", ConditionOperator.EndsWith, Path.GetFileName(location));

            return query;
        }

        private Destination Classify(string location)
        {
            switch (Path.GetExtension(location.ToLowerInvariant()))
            {
                case ".dll":
                    return Destination.Backend;

                case ".js":
                case ".json":
                case ".htm":
                case ".html":
                case ".css":
                case ".jpg":
                case ".jpeg":
                case ".gif":
                case ".png":
                    return Destination.Frontend;
            }

            return Destination.None;
        }

        private EntityReference Execute(Func<QueryExpression, IEnumerable<Entity>> retrieveMultiple, QueryExpression query) =>
            retrieveMultiple(query)?.FirstOrDefault()?.ToEntityReference();

        #endregion Private Methods
    }
}