namespace TicTacToe.Models
{
    /// <summary>
    /// Object representing an error in API usage by requestor.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Message to send as response.
        /// </summary>
        public string message;

        /// <summary>
        /// Error Response constructor
        /// </summary>
        /// <param name="message">Text to send to requestor.</param>
        public ErrorResponse(string message) {
            this.message = message;
        }
    }
}
