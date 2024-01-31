using FluentValidation;
using NzbDrone.Core.Annotations;
using NzbDrone.Core.ThingiProvider;
using NzbDrone.Core.Validation;

namespace NzbDrone.Core.Notifications.Emby
{
    public class MediaBrowserSettingsValidator : AbstractValidator<MediaBrowserSettings>
    {
        public MediaBrowserSettingsValidator()
        {
            RuleFor(c => c.Address).ValidAddress();
            RuleFor(c => c.ApiKey).NotEmpty();
        }
    }

    public class MediaBrowserSettings : IProviderConfig
    {
        private static readonly MediaBrowserSettingsValidator Validator = new MediaBrowserSettingsValidator();

        [FieldDefinition(0, Label = "NotificationsSettingsAddress", HelpText = "NotificationsSettingsAddressHelpText")]
        [FieldToken(TokenField.HelpText, "NotificationsSettingsAddress", "serviceName", "Emby / Jellyfin")]
        public string Address { get; set; } = "http://localhost:8096/mediabrowser";

        [FieldDefinition(1, Label = "ApiKey", Privacy = PrivacyLevel.ApiKey)]
        public string ApiKey { get; set; }

        [FieldDefinition(2, Label = "NotificationsEmbySettingsSendNotifications", HelpText = "NotificationsEmbySettingsSendNotificationsHelpText", Type = FieldType.Checkbox)]
        public bool Notify { get; set; }

        [FieldDefinition(3, Label = "NotificationsSettingsUpdateLibrary", HelpText = "NotificationsEmbySettingsUpdateLibraryHelpText", Type = FieldType.Checkbox)]
        public bool UpdateLibrary { get; set; }

        public NzbDroneValidationResult Validate()
        {
            return new NzbDroneValidationResult(Validator.Validate(this));
        }
    }
}
