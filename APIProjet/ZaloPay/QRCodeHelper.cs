using System.Collections.Generic;
using QRCoder;

namespace APIProject.ZaloPay
{
    public class QRCodeHelper
    {
        private static readonly QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();

        public static BitmapByteQRCode CreateQRBase64Code(string text)
        {
            var qrCodeData = qrCodeGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new BitmapByteQRCode(qrCodeData);
            return qrCode;
        }

        public static string CreateQRCodeBase64Image(string text)
        {
            var qrCode = CreateQRBase64Code(text);
            var bytes = qrCode.GetGraphic(5);
            return "data:image/png;base64," + Convert.ToBase64String(bytes);
        }
    }
}