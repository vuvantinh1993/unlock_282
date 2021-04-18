using OtpNet;

namespace unlock_282
{
    public static class _2Fa
    {
        public static string Lay2FaFB(string twoFactorAuthSeed)
        {
            twoFactorAuthSeed = twoFactorAuthSeed.Replace(" ", "");
            var otpKeyBytes = Base32Encoding.ToBytes(twoFactorAuthSeed);
            // Instanticate the generator's class
            var totp = new Totp(otpKeyBytes);
            // Compute the 2FA code. Easy.
            var twoFactorCode = totp.ComputeTotp();
            return twoFactorCode;
        }
    }
}
