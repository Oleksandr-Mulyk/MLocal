namespace Local.Web.Components.ToDo
{
    public static class ToDoRoute
    {
        public const string TODO_AREA = "todo";

        public const string TODO_LIST_PAGE = TODO_AREA + "s";

        public const string CREATE_TODO_PAGE = TODO_AREA + "/create";

        public const string UPDATE_TODO_PAGE = TODO_AREA + "/update/{" + TODO_ID_PARAM_NAME + "}";

        public const string DELETE_TODO_PAGE = TODO_AREA + "/delete/{" + TODO_ID_PARAM_NAME + "}";

        public const string TODO_ID_PARAM_NAME = "id:int";
    }
}
