using Classes.Common;
using System;
using System.Linq;

namespace Classes.Utilities
{
    public class Utility
    {
        public static TEntity CopyEntity<TEntity>(TEntity source) where TEntity : class, new()
        {

            // Get properties from EF that are read/write and not marked witht he NotMappedAttribute
            var sourceProperties = typeof(TEntity)
                                    .GetProperties()
                                    .Where(p => p.CanRead && p.CanWrite &&
                                                p.GetCustomAttributes(
                                                    typeof(System.ComponentModel.DataAnnotations.Schema.
                                                        NotMappedAttribute), true).Length == 0);
            var notVirtualProperties = sourceProperties.Where(p => !p.GetGetMethod().IsVirtual);
            var newObj = new TEntity();

            foreach (var property in notVirtualProperties)
            {
                // Copy value
                property.SetValue(newObj, property.GetValue(source, null), null);

            }
            return newObj;
        }

        
    }

    public class AlertMessage
    {
        public Enums.AlertMessageType MessageType { get; set; }
        private string _MessageContent;
        public string MessageContent
        {
            get
            {
                if (_MessageContent == null)
                {
                    _MessageContent = GetAlertMessage();
                }

                return _MessageContent;
            }
            set
            {
                _MessageContent = value;
            }
        }
        public int? TransactionCount { get; set; }
        public Enums.Transactions Transaction { get; set; }


        internal string GetAlertMessage()
        {
            var message = "";
            switch (Transaction)
            {
                case Enums.Transactions.Create:
                    switch (MessageType)
                    {
                        case Enums.AlertMessageType.Success:
                            message = this.TransactionCount + " " + Resources.Resource.AlertInsertSuccessMessage;
                            break;
                        case Enums.AlertMessageType.Error:
                            message = this.TransactionCount + " " + Resources.Resource.AlertInsertErrorMessage;
                            break;
                        default:
                            break;
                    }
                    break;
                case Enums.Transactions.Edit:
                    switch (MessageType)
                    {
                        case Enums.AlertMessageType.Success:
                            message = this.TransactionCount + " " + Resources.Resource.AlertEditSuccessMessage;
                            break;
                        case Enums.AlertMessageType.Error:
                            message = this.TransactionCount + " " + Resources.Resource.AlertEditErrorMessage;
                            break;
                        default:
                            break;
                    }
                    break;
                case Enums.Transactions.Delete:
                    switch (MessageType)
                    {
                        case Enums.AlertMessageType.Success:
                            message = this.TransactionCount + " " + Resources.Resource.AlertDeleteSuccessMessage;
                            break;
                        case Enums.AlertMessageType.Error:
                            message = this.TransactionCount + " " + Resources.Resource.AlertDeleteErrorMessage;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return message;
        }
        internal static string GetAlertMessage(Enums.AlertMessageType alertType, int TransactionCount, string messageContent)
        {
            return TransactionCount + " " + messageContent;
        }
    }

    public static class StringExtensions
    {
        public static bool Contains(this string source, string value, StringComparison compparison)
        {
            return source?.IndexOf(value, compparison) >= 0;
        }
    }
}