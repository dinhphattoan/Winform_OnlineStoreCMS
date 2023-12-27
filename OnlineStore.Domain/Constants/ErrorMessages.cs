namespace OnlineStore.Domain.Constants
{
	public static class ErrorMessages
	{
		public const String FAILED_TO = "Failed to";
		public const String NOT_FOUND = "not found";
		public const String CREATE = "create";
		public const String UPDATE = "update";
		public const String DELETE = "delete";
		public const String DISABLE = "disable";
		public const String ENABLE = "enable";
		public const String INVALID = "Invalid";
		public const String DATA = "Data";

		public const String INVALID_DATA = $"{INVALID} {DATA}";

		public const String PRODUCT = "Product";
		public const String PRODUCT_NOT_FOUND = $"{PRODUCT} {NOT_FOUND}";

		public const String CATEGORY = "Category";
		public const String CATEGORY_NOT_FOUND = $"{CATEGORY} {NOT_FOUND}";
		public const String FAILED_TO_CREATE_CATEGORY = $"{FAILED_TO} {CREATE} {CATEGORY}";

		public const string USER = "User";
		public const string USER_NOT_FOUND = $"{USER} {NOT_FOUND}";
		public const string FAILED_TO_CREATE_USER = $"{FAILED_TO} {CREATE} {USER}";
		public const string FAILED_TO_UPDATE_USER = $"{FAILED_TO} {UPDATE} {USER}";
		public const string FAILED_TO_DELETE_USER = $"{FAILED_TO} {DELETE} {USER}";
		public const string FAILED_TO_DISABLE_USER = $"{FAILED_TO} {DISABLE} {USER}";
		public const string FAILED_TO_ENABLE_USER = $"{FAILED_TO} {ENABLE} {USER}";
	}
}
