namespace Local.Web.Components.Admin.Users
{
    public static class AdminUserRoute
    {
        public const string USER_AREA = AdminRoute.AREA + "/user";

        public const string USER_LIST_PAGE = USER_AREA + "s";

        public const string CREATE_USER_PAGE = USER_AREA + "/create";

        public const string UPDATE_USER_PAGE = USER_AREA + "/update/{" + USER_ID_PARAM_NAME + "}";

        public const string DELETE_USER_PAGE = USER_AREA + "/delete/{" + USER_ID_PARAM_NAME + "}";

        public const string USER_ID_PARAM_NAME = "id";

        public const string ROLE_AREA = AdminRoute.AREA + "/role";

        public const string ROLE_LIST_PAGE = ROLE_AREA + "s";

        public const string CREATE_ROLE_PAGE =ROLE_AREA + "/create";

        public const string UPDATE_ROLE_PAGE =ROLE_AREA + "/update/{" + ROLE_ID_PARAM_NAME + "}";

        public const string DELETE_ROLE_PAGE =ROLE_AREA + "/delete/{" + ROLE_ID_PARAM_NAME + "}";

        public const string ROLE_ID_PARAM_NAME = "id";
    }
}
