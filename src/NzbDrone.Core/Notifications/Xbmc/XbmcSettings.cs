using FluentValidation;
using NzbDrone.Core.Annotations;
using NzbDrone.Core.ThingiProvider;
using NzbDrone.Core.Validation;

namespace NzbDrone.Core.Notifications.Xbmc
{
    public class XbmcSettingsValidator : AbstractValidator<XbmcSettings>
    {
        public XbmcSettingsValidator()
        {
            RuleFor(c => c.Address).ValidAddress();
            RuleFor(c => c.DisplayTime).GreaterThanOrEqualTo(2);
        }
    }

    public class XbmcSettings : IProviderConfig
    {
        private static readonly XbmcSettingsValidator Validator = new XbmcSettingsValidator();

        [FieldDefinition(0, Label = "NotificationsSettingsAddress", HelpText = "NotificationsSettingsAddressHelpText")]
        [FieldToken(TokenField.HelpText, "NotificationsSettingsAddress", "serviceName", "Kodi")]
        public string Address { get; set; }

        [FieldDefinition(1, Label = "Username", Privacy = PrivacyLevel.UserName)]
        public string Username { get; set; }

        [FieldDefinition(2, Label = "Password", Type = FieldType.Password, Privacy = PrivacyLevel.Password)]
        public string Password { get; set; }

        [FieldDefinition(3, Label = "NotificationsKodiSettingsDisplayTime", HelpText = "NotificationsKodiSettingsDisplayTimeHelpText")]
        public int DisplayTime { get; set; } = 5;

        [FieldDefinition(4, Label = "NotificationsKodiSettingsGuiNotification", Type = FieldType.Checkbox)]
        public bool Notify { get; set; }

        [FieldDefinition(5, Label = "NotificationsSettingsUpdateLibrary", HelpText = "NotificationsKodiSettingsUpdateLibraryHelpText", Type = FieldType.Checkbox)]
        public bool UpdateLibrary { get; set; }

        [FieldDefinition(6, Label = "NotificationsKodiSettingsCleanLibrary", HelpText = "NotificationsKodiSettingsCleanLibraryHelpText", Type = FieldType.Checkbox)]
        public bool CleanLibrary { get; set; }

        [FieldDefinition(7, Label = "NotificationsKodiSettingAlwaysUpdate", HelpText = "NotificationsKodiSettingAlwaysUpdateHelpText", Type = FieldType.Checkbox)]
        public bool AlwaysUpdate { get; set; }

        public NzbDroneValidationResult Validate()
        {
            return new NzbDroneValidationResult(Validator.Validate(this));
        }
    }
}
