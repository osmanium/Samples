using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture
{
    public class ValidatableBindableBase : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region Private Fields

        private IDictionary<string, ReadOnlyCollection<string>> _errors = new Dictionary<string, ReadOnlyCollection<string>>();

        #endregion

        #region Properties

        public bool IsValidationEnabled { get; set; }

        #endregion

        public ValidatableBindableBase(bool isValidationEnabled = true)
        {
            IsValidationEnabled = isValidationEnabled;
        }

        public bool ValidateProperties()
        {
            _errors.Clear();

            var propertiesWithChangedErrors = new List<string>();

            // Get all the properties decorated with the ValidationAttribute attribute.
            var propertiesToValidate = this.GetType()
                                                        .GetRuntimeProperties()
                                                        .Where(c => c.GetCustomAttributes(typeof(ValidationAttribute)).Any());

            foreach (PropertyInfo propertyInfo in propertiesToValidate)
            {
                var propertyErrors = new List<string>();
                TryValidateProperty(propertyInfo, propertyErrors);

                // If the errors have changed, save the property name to notify the update at the end of this method.
                bool errorsChanged = SetPropertyErrors(propertyInfo.Name, propertyErrors);
                if (errorsChanged && !propertiesWithChangedErrors.Contains(propertyInfo.Name))
                {
                    propertiesWithChangedErrors.Add(propertyInfo.Name);
                }
            }



            // Notify each property whose set of errors has changed since the last validation. 
            foreach (string propertyName in propertiesWithChangedErrors)
            {
                OnErrorsChanged(propertyName);
                OnPropertyChanged(propertyName);
            }

            return _errors.Values.Count == 0;
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertySelector)
        {
            var memberExpression = propertySelector.Body as MemberExpression;
            if (memberExpression != null)
            {
                OnPropertyChanged(memberExpression.Member.Name);
            }
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            var result = false;


            try
            {
                storage = value;
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }



            if (result && !string.IsNullOrEmpty(propertyName))
            {
                if (IsValidationEnabled)
                {

                    var propertiesWithChangedErrors = new List<string>();

                    // Get all the properties decorated with the ValidationAttribute attribute.
                    var propertyToValidate = this.GetType()
                                                                .GetRuntimeProperties()
                                                                .Where(c => c.Name == propertyName).FirstOrDefault();


                    var propertyErrors = new List<string>();
                    TryValidateProperty(propertyToValidate, propertyErrors);

                    // If the errors have changed, save the property name to notify the update at the end of this method.
                    bool errorsChanged = SetPropertyErrors(propertyToValidate.Name, propertyErrors);
                    if (errorsChanged && !propertiesWithChangedErrors.Contains(propertyToValidate.Name))
                    {
                        propertiesWithChangedErrors.Add(propertyToValidate.Name);
                    }

                    // Notify each property whose set of errors has changed since the last validation. 
                    foreach (string propertyName_tobenotified in propertiesWithChangedErrors)
                    {
                        OnErrorsChanged(propertyName_tobenotified);
                    }

                    OnPropertyChanged(propertyName);
                }
            }
            return result;
        }

        private bool SetPropertyErrors(string propertyName, IList<string> propertyNewErrors)
        {
            bool errorsChanged = false;

            // If the property does not have errors, simply add them
            if (!_errors.ContainsKey(propertyName))
            {
                if (propertyNewErrors.Count > 0)
                {
                    _errors.Add(propertyName, new ReadOnlyCollection<string>(propertyNewErrors));
                    errorsChanged = true;
                }
            }
            else
            {
                // If the property has errors, check if the number of errors are different.
                // If the number of errors is the same, check if there are new ones
                if (propertyNewErrors.Count != _errors[propertyName].Count || _errors[propertyName].Intersect(propertyNewErrors).Count() != propertyNewErrors.Count())
                {
                    if (propertyNewErrors.Count > 0)
                    {
                        _errors[propertyName] = new ReadOnlyCollection<string>(propertyNewErrors);
                    }
                    else
                    {
                        _errors.Remove(propertyName);
                    }

                    errorsChanged = true;
                }
            }

            return errorsChanged;
        }

        private bool TryValidateProperty(PropertyInfo propertyInfo, List<string> propertyErrors)
        {
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var context = new ValidationContext(this) { MemberName = propertyInfo.Name };
            var propertyValue = propertyInfo.GetValue(this);

            // Validate the property
            bool isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateProperty(propertyValue, context, results);

            if (results.Any())
            {
                propertyErrors.AddRange(results.Select(c => c.ErrorMessage));
            }

            return isValid;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyDataErrorInfo

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return _errors;
            }

            var propertyInfo = this.GetType().GetRuntimeProperty(propertyName);

            if (propertyInfo == null)
            {
                throw new ArgumentException(String.Format("Doesn't have a property {0}"), propertyName);
            }

            var propertyErrors = new List<string>();
            bool isValid = TryValidateProperty(propertyInfo, propertyErrors);
            bool errorsChanged = SetPropertyErrors(propertyInfo.Name, propertyErrors);

            if (errorsChanged)
            {
                OnErrorsChanged(propertyName);
                OnPropertyChanged(propertyName);
            }

            if (_errors.ContainsKey(propertyName))
                return _errors[propertyName];
            else return null;
        }

        public bool HasErrors
        {
            get
            {
                return _errors.Any();
            }
        }

        private void OnErrorsChanged(string propertyName)
        {
            var eventHandler = ErrorsChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        #endregion

    }
}
