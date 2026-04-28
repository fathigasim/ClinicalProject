namespace ClinicProjectApi.Dtos
{
    // Only the access token goes in the response body —
    // refresh token travels exclusively via HttpOnly cookie
    public record AccessTokenResponseDto(string AccessToken);

}
