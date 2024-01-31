using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NzbDrone.Common.Serializer;
using NzbDrone.Core.Datastore.Migration;
using NzbDrone.Core.Test.Framework;

namespace NzbDrone.Core.Test.Datastore.Migration
{
    [TestFixture]
    public class url_unificationFixture : MigrationTest<url_unification>
    {
        [Test]
        public void should_mediaserver_create_address()
        {
            var db = WithMigrationTestDb(c =>
            {
                c.Insert.IntoTable("Notifications").Row(new
                {
                    OnGrab = true,
                    OnDownload = true,
                    OnUpgrade = true,
                    OnHealthIssue = true,
                    IncludeHealthWarnings = true,
                    OnRename = true,
                    OnMovieDelete = false,
                    Name = "Emby",
                    Implementation = "MediaBrowser",
                    Tags = "[]",
                    Settings = new MediaBrowserSettings235
                    {
                        Host = "localhost",
                        Port = 8096,
                        UseSsl = false
                    }.ToJson(),
                    ConfigContract = "MediaBrowserSettings"
                });
            });

            var items = db.Query<NotificationDefinition236>("SELECT * FROM \"Notifications\"");

            items.Should().HaveCount(1);
            items.First().Implementation.Should().Be("MediaBrowser");
            items.First().ConfigContract.Should().Be("MediaBrowserSettings");
            items.First().Settings.Address.Should().Be("http://localhost:8096/mediabrowser");
        }

        [Test]
        public void should_mediabrowser_fix_ipv6_hosts()
        {
            var db = WithMigrationTestDb(c =>
            {
                c.Insert.IntoTable("Notifications").Row(new
                {
                    OnGrab = true,
                    OnDownload = true,
                    OnUpgrade = true,
                    OnHealthIssue = true,
                    IncludeHealthWarnings = true,
                    OnRename = true,
                    OnMovieDelete = false,
                    Name = "Emby",
                    Implementation = "MediaBrowser",
                    Tags = "[]",
                    Settings = new MediaBrowserSettings235
                    {
                        Host = ":1",
                        Port = 443,
                        UseSsl = true
                    }.ToJson(),
                    ConfigContract = "MediaBrowserSettings"
                });
            });

            var items = db.Query<NotificationDefinition236>("SELECT * FROM \"Notifications\"");

            items.Should().HaveCount(1);
            items.First().Implementation.Should().Be("MediaBrowser");
            items.First().ConfigContract.Should().Be("MediaBrowserSettings");
            items.First().Settings.Address.Should().Be("https://[:1]:443/mediabrowser");
        }
    }

    public class NotificationDefinition236
    {
        public int Id { get; set; }
        public string Implementation { get; set; }
        public string ConfigContract { get; set; }
        public MediaBrowserSettings236 Settings { get; set; }
        public string Name { get; set; }
        public bool OnGrab { get; set; }
        public bool OnDownload { get; set; }
        public bool OnUpgrade { get; set; }
        public bool OnRename { get; set; }
        public bool OnMovieDelete { get; set; }
        public bool OnMovieFileDelete { get; set; }
        public bool OnMovieFileDeleteForUpgrade { get; set; }
        public bool OnHealthIssue { get; set; }
        public bool OnApplicationUpdate { get; set; }
        public bool OnManualInteractionRequired { get; set; }
        public bool OnMovieAdded { get; set; }
        public bool OnHealthRestored { get; set; }
        public bool IncludeHealthWarnings { get; set; }
        public List<int> Tags { get; set; }
    }

    public class MediaBrowserSettings235
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string ApiKey { get; set; }
        public bool Notify { get; set; }
        public bool UpdateLibrary { get; set; }
    }

    public class MediaBrowserSettings236
    {
        public string Address { get; set; }
        public string ApiKey { get; set; }
        public bool Notify { get; set; }
        public bool UpdateLibrary { get; set; }
    }
}
