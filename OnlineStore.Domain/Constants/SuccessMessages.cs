namespace OnlineStore.Domain.Constants
{
	public static class SuccessMessages
	{
		public const String SUCCESS = "Successully";
		public const String UPDATE = "update";
		public const String CREATE = "created";
		public const String DELETE = "delete";

		public const String UPDATE_SUCCESS = UPDATE + " " + SUCCESS;
		public const String CREATE_SUCCESS = CREATE + " " + SUCCESS;
		public const String DELETE_SUCCESS = DELETE + " " + SUCCESS;

		public const String PRODUCT = "Product";
		public const String SUCCESS_UPDATE_PRODUCT = SUCCESS + " " + UPDATE + " " + PRODUCT;
		public const String SUCCESS_CREATE_PRODUCT = SUCCESS + " " + CREATE + " " + PRODUCT;
		public const String SUCCESS_DELETE_PRODUCT = SUCCESS + " " + DELETE + " " + PRODUCT;

		public const String CATEGORY = "category";
		public const String SUCCESS_UPDATE_CATEGORY = SUCCESS + " " + UPDATE + " " + CATEGORY;
		public const String SUCCESS_CREATE_CATEGORY = SUCCESS + " " + CREATE + " " + CATEGORY;
		public const String SUCCESS_DELETE_CATEGORY = SUCCESS + " " + DELETE + " " + CATEGORY;

		public const string User = "User";
		public const string SUCCESS_UPDATE_USER = SUCCESS + " " + UPDATE + " " + User;
		public const string SUCCESS_CREATE_USER = SUCCESS + " " + CREATE + " " + User;
		public const string SUCCESS_DELETE_USER = SUCCESS + " " + DELETE + " " + User;

	}
}
