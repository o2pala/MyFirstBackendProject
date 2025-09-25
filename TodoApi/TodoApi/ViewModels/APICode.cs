namespace TodoApi.ViewModels
{
    public enum APICode
    {
        Success = 200,
        InvalidArgument = 400,
        Unauthenticated = 401,
        Forbidden = 403,
        NotFound = 404,
        Fail = 500,
        Duplicated = 900,
        DataIsReferenced = 901,
        NonrepeatableRead = 902,
        CannotDeleteSystemData = 903,
        NoUserPagedData = 904,
        Desynchronization = 905,
        ReconnectEmailMismatched = 906,
    }
}
