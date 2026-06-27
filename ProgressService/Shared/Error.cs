namespace ProgressService.Shared
{
    public class Error
    {
        public string Code { get; }
        public string Description { get; }
        public ErrorType Type { get; }

        private Error(string code, string description, ErrorType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }

        #region Static Factory Methods to Create Error

        public static Error Failure(string Code = "General.Failure", string Description = "A General Failure Has Occured")
        {
            return new Error(Code, Description, ErrorType.Failure);
        }
        public static Error Validation(string Code = "General.Validation", string Description = "A General Validation Has Occured")
        {
            return new Error(Code, Description, ErrorType.Validation);
        }
        public static Error NotFound(string Code = "General.NotFound", string Description = "A General NotFound Has Occured")
        {
            return new Error(Code, Description, ErrorType.NotFound);
        }
        public static Error Forbidden(string Code = "General.Forbidden", string Description = "A General Forbidden Has Occured")
        {
            return new Error(Code, Description, ErrorType.Forbidden);
        }
        public static Error InvalidCredentials(string Code = "General.InvalidCredentials", string Description = "A General InvalidCredentials Has Occured")
        {
            return new Error(Code, Description, ErrorType.InvalidCredentials);
        }
        #endregion
    }
}
