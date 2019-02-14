namespace TicTacToe.Models {
    /// <summary>
    /// Object representing an error in API usage by requestor.
    /// </summary>
    public class ErrorResponse {
        /// <summary>
        /// Message to send as response.
        /// </summary>
        public string message;

        /// <summary>
        /// Error code.
        /// </summary>
        public int code;

        /// <summary>
        /// Error Response constructor
        /// </summary>
        /// <param name="message">Text to send to requestor.</param>
        /// <param name="code">Error code to send to requestor. 1 indicates an improper input, 0 is a catch-all.</param>
        public ErrorResponse (int code, string message) {
            this.code = code;
            this.message = message;
        }
    }
}
