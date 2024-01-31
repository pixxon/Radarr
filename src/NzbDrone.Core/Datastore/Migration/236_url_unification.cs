using System.Data;
using FluentMigrator;
using Newtonsoft.Json.Linq;
using NzbDrone.Common.Extensions;
using NzbDrone.Common.Serializer;
using NzbDrone.Core.Datastore.Migration.Framework;

namespace NzbDrone.Core.Datastore.Migration
{
    [Migration(236)]
    public class url_unification : NzbDroneMigrationBase
    {
        protected override void MainDbUpgrade()
        {
            Execute.WithConnection(CreateMediaBrowserAddress);
            Execute.WithConnection(CreateSignalAddress);
            Execute.WithConnection(CreatePlexServerAddress);
            Execute.WithConnection(CreateXbmcAddress);
        }

        private string fixIPv6(string host)
        {
            // copied from StringExtensions
            return host.Contains(':') ? $"[{host}]" : host;
        }

        private void CreateMediaBrowserAddress(IDbConnection conn, IDbTransaction tran)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.Transaction = tran;
                cmd.CommandText = "SELECT \"Id\", \"Settings\" FROM \"Notifications\" WHERE \"Implementation\" = 'MediaBrowser'";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var settings = reader.GetString(1);

                        if (settings.IsNotNullOrWhiteSpace())
                        {
                            var jsonObject = Json.Deserialize<JObject>(settings);

                            var scheme = jsonObject["useSsl"].Value<bool>() ? "https" : "http";
                            var host = jsonObject["host"].Value<string>();
                            var port = jsonObject["port"].Value<int>();

                            // TODO if port 80 or 443 handle specially?
                            var url = $@"{scheme}://{fixIPv6(host)}:{port}/mediabrowser";

                            jsonObject.Remove("host");
                            jsonObject.Remove("port");
                            jsonObject.Remove("useSsl");
                            jsonObject.Remove("address");
                            jsonObject.AddFirst(new JProperty("address", url));
                            settings = jsonObject.ToJson();

                            using (var updateCmd = conn.CreateCommand())
                            {
                                updateCmd.Transaction = tran;
                                updateCmd.CommandText = "UPDATE Notifications SET Settings = ? WHERE Id = ?";
                                updateCmd.AddParameter(settings);
                                updateCmd.AddParameter(id);
                                updateCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }

        private void CreateSignalAddress(IDbConnection conn, IDbTransaction tran)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.Transaction = tran;
                cmd.CommandText = "SELECT \"Id\", \"Settings\" FROM \"Notifications\" WHERE \"Implementation\" = 'Signal'";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var settings = reader.GetString(1);

                        if (settings.IsNotNullOrWhiteSpace())
                        {
                            var jsonObject = Json.Deserialize<JObject>(settings);

                            var scheme = jsonObject["useSsl"].Value<bool>() ? "https" : "http";
                            var host = jsonObject["host"].Value<string>();
                            var port = jsonObject["port"].Value<int>();

                            // TODO if port 80 or 443 handle specially?
                            var url = $@"{scheme}://{fixIPv6(host)}:{port}";

                            jsonObject.Remove("host");
                            jsonObject.Remove("port");
                            jsonObject.Remove("useSsl");
                            jsonObject.Remove("address");
                            jsonObject.AddFirst(new JProperty("address", url));
                            settings = jsonObject.ToJson();

                            using (var updateCmd = conn.CreateCommand())
                            {
                                updateCmd.Transaction = tran;
                                updateCmd.CommandText = "UPDATE Notifications SET Settings = ? WHERE Id = ?";
                                updateCmd.AddParameter(settings);
                                updateCmd.AddParameter(id);
                                updateCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }

        private void CreatePlexServerAddress(IDbConnection conn, IDbTransaction tran)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.Transaction = tran;
                cmd.CommandText = "SELECT \"Id\", \"Settings\" FROM \"Notifications\" WHERE \"Implementation\" = 'PlexServer'";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var settings = reader.GetString(1);

                        if (settings.IsNotNullOrWhiteSpace())
                        {
                            var jsonObject = Json.Deserialize<JObject>(settings);

                            var scheme = jsonObject["useSsl"].Value<bool>() ? "https" : "http";
                            var host = jsonObject["host"].Value<string>();
                            var port = jsonObject["port"].Value<int>();

                            // TODO if port 80 or 443 handle specially?
                            var url = $@"{scheme}://{fixIPv6(host)}:{port}";

                            jsonObject.Remove("host");
                            jsonObject.Remove("port");
                            jsonObject.Remove("useSsl");
                            jsonObject.Remove("address");
                            jsonObject.AddFirst(new JProperty("address", url));
                            settings = jsonObject.ToJson();

                            using (var updateCmd = conn.CreateCommand())
                            {
                                updateCmd.Transaction = tran;
                                updateCmd.CommandText = "UPDATE Notifications SET Settings = ? WHERE Id = ?";
                                updateCmd.AddParameter(settings);
                                updateCmd.AddParameter(id);
                                updateCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }

        private void CreateXbmcAddress(IDbConnection conn, IDbTransaction tran)
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.Transaction = tran;
                cmd.CommandText = "SELECT \"Id\", \"Settings\" FROM \"Notifications\" WHERE \"Implementation\" = 'Xbmc'";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var settings = reader.GetString(1);

                        if (settings.IsNotNullOrWhiteSpace())
                        {
                            var jsonObject = Json.Deserialize<JObject>(settings);

                            var scheme = jsonObject["useSsl"].Value<bool>() ? "https" : "http";
                            var host = jsonObject["host"].Value<string>();
                            var port = jsonObject["port"].Value<int>();

                            // TODO if port 80 or 443 handle specially?
                            var url = $@"{scheme}://{fixIPv6(host)}:{port}";

                            jsonObject.Remove("host");
                            jsonObject.Remove("port");
                            jsonObject.Remove("useSsl");
                            jsonObject.Remove("address");
                            jsonObject.AddFirst(new JProperty("address", url));
                            settings = jsonObject.ToJson();

                            using (var updateCmd = conn.CreateCommand())
                            {
                                updateCmd.Transaction = tran;
                                updateCmd.CommandText = "UPDATE Notifications SET Settings = ? WHERE Id = ?";
                                updateCmd.AddParameter(settings);
                                updateCmd.AddParameter(id);
                                updateCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }
    }
}