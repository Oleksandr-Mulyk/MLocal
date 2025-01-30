namespace Local.Web.Components.Admin.Users
{
    public static class AdminUserRoute
    {
        public const string AREA = AdminRoute.AREA + "/user";

        public const string USER_LIST_PAGE = AREA + "s";

        public const string CREATE_USER_PAGE = AREA + "/create";

        public const string UPDATE_USER_PAGE = AREA + "/update/{" + USER_ID_PARAM_NAME + "}";

        public const string DELETE_USER_PAGE = AREA + "/delete/{" + USER_ID_PARAM_NAME + "}";

        public const string USER_ID_PARAM_NAME = "id";
    }
}
