using System;

namespace DonationBoard
{
    public interface IValidationConstraint<T>
    {
        Func<bool> Validate { get; }
        string Message { get; }
    }

    public class RequiredConstraint<T> : IValidationConstraint<T>
    {
        public RequiredConstraint(string propertyName, object propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public string PropertyName { get; set; }
        public object PropertyValue { get; set; }

        public Func<bool> Validate => ValidateProperty;

        private bool ValidateProperty()
        {
            if (PropertyValue != null)
            {
                return !string.IsNullOrEmpty(PropertyValue.ToString());
            }
            return false;
        }

        public string Message => $"Property '{PropertyName}' cannot be empty.";
    }

    public class RequiredGreaterThanZeroConstraint<T> : IValidationConstraint<T>
    {
        public RequiredGreaterThanZeroConstraint(string propertyName, object propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public string PropertyName { get; set; }
        public object PropertyValue { get; set; }

        public Func<bool> Validate => ValidateProperty;

        private bool ValidateProperty()
        {
            if (PropertyValue != null)
            {
                if (double.TryParse(PropertyValue.ToString(), out double d))
                {
                    return d > 0;
                }
            }
            return false;
        }

        public string Message => $"Property '{PropertyName}' cannot be empty.";
    }
}
