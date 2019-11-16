using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace JwtAuthenticationHelper.Types
{
    /// <summary>
    /// A structure containting the various options required
    /// to generate a valid Json Web Token
    /// </summary>
    public sealed class TokenOptions
    {
        public TokenOptions() { }
        /// <summary>
        /// Creates a new instance of <see cref="TokenOptions"/>
        /// </summary>
        /// <param name="issuer">
        /// Required. Issuer of the token, usually,
        /// your web application URL but could be any string
        /// </param>
        /// <param name="audience">
        /// Required. Audience of the token i.e. who the token is for.
        /// Could be any string
        /// </param>
        /// <param name="signingKey">
        /// Required. An instance of <see cref="SecurityKey"/> containing
        /// the encoded 128-bit string.
        /// Any string that is sufficiently long and unguessable will do.
        /// </param>
        /// <param name="tokenExpiryInMinutes">Defaults to 5 minutes
        /// but can be longer or shorter.</param>
        public TokenOptions(string issuer, string audience, string rawSigningKey, int tokenExpiryInMinutes = 5)
        {
            if (string.IsNullOrWhiteSpace(audience))
            {
                throw new ArgumentNullException(
                    $"{nameof(Audience)} is mandatory in order to generate a JWT!");
            }

            if (string.IsNullOrWhiteSpace(issuer))
            {
                throw new ArgumentNullException(
                    $"{nameof(Issuer)} is mandatory in order to generate a JWT!");
            }

            if (string.IsNullOrWhiteSpace(rawSigningKey))
            {
                throw new ArgumentNullException($"{nameof(SigningKey)} is mandatory in order to generate a JWT!");
            }

            Audience = audience;
            Issuer = issuer;
            RawSigningKey = rawSigningKey;
            TokenExpiryInMinutes = tokenExpiryInMinutes;
        }

        public string RawSigningKey { get; set; }
        public SecurityKey SigningKey => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(RawSigningKey));
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int TokenExpiryInMinutes { get; set; }
    }
}