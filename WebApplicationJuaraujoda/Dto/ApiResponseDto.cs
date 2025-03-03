namespace Dto
{
    /// <summary>
    /// DTO pour la réponse d'une API.
    /// </summary>
    /// <typeparam name="T">Type du résultat retourné.</typeparam>
    public class ApiResponseDto<T>
    {
        public T Result { get; set; }
        public int Id { get; set; }
        public object Exception { get; set; }
        public int Status { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsCompletedSuccessfully { get; set; }
        public int CreationOptions { get; set; }
        public object AsyncState { get; set; }
        public bool IsFaulted { get; set; }
    }
}
