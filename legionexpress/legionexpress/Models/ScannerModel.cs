using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Scandit.DataCapture.Barcode.Capture.Unified;
using Scandit.DataCapture.Barcode.Data.Unified;
using Scandit.DataCapture.Core.Capture.Unified;
using Scandit.DataCapture.Core.Common.Geometry.Unified;
using Scandit.DataCapture.Core.Source.Unified;
namespace legionexpress.Models
{
    public class ScannerModel
    {
        // Enter your Scandit License key here.
        // public const string SCANDIT_LICENSE_KEY = "AT7xGzWXFO6EA8TRlzw8LJ057dQ1G0VzHlxMaPlWSMFvR2oR+VLvDjwZhZiHe0oDUTFyMxJUB2jwJt3KQSKo+lh9vWmEWWPyUzOsd8JbPuipaOGTgnjJwIMifxcTSiXKiUV4vwYzn/gKOLpjSURpS2QbNBJwdAmk/tmveTQ8jDfLxKYwH6Dn846INuDwLGbedm5gdGRhvpsQqUVpnKtUM5NWi852RetGsCA2rw1Ys/sF+9FFs1X8wRh1sfJCXXSzVB5P/dfeWREcNO9wN35Skti2noCHMxGTYqpt1Xi2aZU8U0a3seokJFhxkAUIHm2DAd6WCWUMXbiSOcbzi4/sUK8AMBKPNh+9+op5KxkywtqZdzCvifunUikKSbSXZxPqJK8mfXbg0JeVutj3PqEIMNC8uOZFi/+Exz4qSxvuunVZE5Ms3ruLs+PA/3pFak0Zne0T7kFpgUMFCoa8fqZmrl9Pbfq1IQVXX+sX/Gxlexo+ZYeZherlpf53pxpPxv62kIQlA4DhHen7xCqMAdJaTxiduxCtm02KdXLu5oOtmblHpfUJDCSTKZdVGGRb1H4jmo8z0iRlGMkZPNNrtHd1CEn/Sf5FHC5F+fwkS/Ace85sQmVXqqidOAfnLYMvFLTTooOslq+nziWetJeM0tESkGadqWopK1ZknEmkyBkZ988iiKmQBSac+0jmnX/qTIpQJ4inBe+XBWaCixQppVeMAB2CRJVvwTiLepu9C8orprAG7IgWdp8Ks3HlG6ExGHURIeT0sRnvucNJaQWMZVQtXV93i6GopkYZRK/5z5nNIz4z/NBOkl3zEHReq0bWWrGtaD5zK1A=";
        //public const string SCANDIT_LICENSE_KEY = "AbHhdhE5M12zLKPuDRhvcYtBK8IZEYmAY1QL4ntz64WAfpW4K3QTShALUCUjNpDBqiGqEHpu/QjEXPZ3WG17Sz5R012DfIRX5DdLqkNgz/JfWOetbXfGlIVTCQIzISXCeHtpOtQwHSUUFCSbMQpxXJxZvjTrWCua73H10SteHsaST5tmo0Y2Fsl5MID6WbPSklgG3z1aVEmYEKjDD0nvk+1ZRZH1B3qicHnOnGIQkLmuY+b8mnRoItZuZmAIRbTNVGB5uedpXT56bWKZ6Cs4Fv5zlJU+VyDxslPH3pIDjIl2bsz102J8VWJz7prtehfyzRt6kUBbsg1OE5RkfntFpHJJ5l26daTiVVyq67Fe7AIYHx/PBEsDFiBGPr6QamWVYgtBMoQyB4GrUBjbrhBMqSl+9EaVQOYDvD+hevRc89H+QsgW0xBK7U1ScFX/RTovO0e3NmMr4/PBbfj0XEL4x/hyzUajZ5W9LU8HQaRdssVnVRwI31qTKIN8qbpLcqLjTX/9WeF97RQIQ9h68UrbbiVj6Tq3PJxWeihGRYZpEx8KCq6ZiVdMQBWLfBomJF5ww6/ErI1y7yDYAzLmReq7TjxN5OGhclDgbZ2NHwwn/5F8KQQq2+G3sLKSEuu634zZgP8GQo7JVJAuTWbFaGSxOFMDscQZud6pvl9C/XedIra++Zt9natVpkWbFiASI97ZahbotJoaqLPIswiQkx0gTEUCydL9gEsDkgv82uA4I+EbQGqDq0jZreGj9kc9ee7TTJC6Lfo2Lg0Pt7ulSOV3/KFNg78ix9mcJ91VS0qAQMGlOFAhsV0lVuA1Cx0/cPE81rEiWgSEfAs6d3UAg/GB87IJgNu74+G8SfEcXe3a1CtGm9QpGE3NdSwRwzQg1hcUbEjqg2uI7xCmk91teqRlAW4ZIYmfSW5EX0WpTFMwqheRkV4nE4zglVH5XWbRP7BxozBRet4v+Pg/Dmhxhzls0ut2CgtI5/H49LmFVSdbmqOlUzctlQtOwurHHnO5d+h4kYDDyIuoQ/vR6hrPPGlnpFzxxpBx5B9lUt7/m7ICzNZMwNmNWAbQmGK0sinCKDhmy2qL53N5gU5VOFLnZgIHq55sDCoRRS7HxHwx2Ox/ZQbdw/VvwnoP+U/5ighExxrQWMm9H52ZweOK1NEVnZFZpxuMWfP/MWAffsQCIxPRbfQGhyZKAHjlYt27u0tMwRjc9NCFIx7dMArEciujgMUtlp+aOtrR";

        //New License
        //Prod
        //PackageName = allegiance.app
        public const string SCANDIT_LICENSE_KEY = "AR8BUJ+BC5mpJRlLSSodN6AUUBj+Jv0GnVc9o2RzDpV4RM6cy3dWU7RqGAyta76oL29Ir/QNY+bIWvLYfX5/4/MadYraV76QjULh8h4mldOrSfWzoScYn7M1M32yK6I+YyIDse1Wmb6Ok+D9LCUGFn+m9d6aicF3Xj5DMvcyw0K2nvI/z1vz/3YKHjbeHAUOlDgSB5XytEMT0jOIMcx/Ncz1tLlIU80Vr7NC6FGCyvyV1M4navGCHMZQ9GZ7PQ+vfBMEYvzf2YAZvL0PY47raaSmU+9M5+2L4IIk4iNjVf8V12ZHh4xQDYEphgDfWvq+g5jkIaxmC9DuL92NIrZ3jVE92OlzsWik3d/T/CUj2XD0V9DygOWmb3IbzYSWEWdm/M+ZC5v8ELV5dntCUaSK25iL4t+DlSmN+QEvsAqvW1+MwCxgxWPiohSYUKMtF9Xavwin+VskJB/lmeRfIOsbshOc3TY5PHIhUaPGYiDBH18xmeUpbvUdtmZBxe6pEtLCuBjKG6PcpW+fpy6+iz/dgSnCElGp6b6TZ0Ari2aKLoKbUzdsPljKVc/taLo31lylqr57GNOQHzwFiNRr7g6pjDEQfL+sZapYDHFZKvODaH4LkUVMb+gWOYKuuHH8m2iedUm+JTO7zg2jAjIHiUrjioUdOL4y5OCI5d/92pI5CbQNA6ODvPBGPLT/RwZGCYsISLyaFCqNtb4gDdUYNPsS6ONUR0C6aJ5Tlx+qkNP5rJe5CacViY/J2IVBXuG0VuC2tn/4qfLVd0xuV5UNnc1pMlzbBxhkP2q53qWvx9aFu0m+hE+w5A==";
        //Dev
        //PackageName = allegiance.apk
        //public const string SCANDIT_LICENSE_KEY = "AdSSqBiWPQR+EY4C+wFf7bMAwyuYKou0NV2179tS0TrcVFoY9FRJzuxIsDR7avX8gH7Aet5KydfTehfjXULcj9ZsJ3stVynUukYG9PJRKWMlSLvTSxHSUsEyYYC/Fg+4QiPysWAovh0XRSnN5QAwKVN+bUDCnNPpIP347APb5i3APvHnmC54BC2svgpVuZoU4i0odeJWiBNJZqIh7Ev8izZSyHJAQY8G8kBGEWMMckxnw3lVKNt6NGw6x3UsroDwU9/CEWUfPeyt4Xs0pl3YMcGx4ycl5PXSicAI5JqHupDtOoVAs3buSg9y4tl7FNO97cov+ryW5yPZ2RKJJ0iyZtoEibBai2dK8lKA5a5sTwQ2pshKAK0wAMf4nFbaZKILQvxJzj1Om8v/RoCznlXm5RliyHPE62wENkvl+3tzNNRagPsBRV5Fr1dY4IIbFO8yUSErPRuG+HdX87kayejfTjf0NSWdf4scEDHdJCNLaA2+1m3OvZk22j8JVcjLphFZ0mV1vvOABw1rj6LM5IEo6vdf65xtIKNeLHnjg+6Wln81S9WBCHHFG6cyyUTbp+6XFUQCLiQ9lfQlSIFdpeZ2DmZjMGc+HHETqoABJ9qCPYvTYzUcY54EcbZC92yC2sETDbb3yFv8ADg7mjNhSOPIVELdyeg9l9aL7VTpMdcv8bgNSzvBkA74JFJzY6d4movPNwvy8MPWPKNoiSajPsJSSOz9c+0af5O9B6g22GKgkMCZmw6QPoUifMpWiWTZKSdwsofAvYYtKI1472MmiWzxjhzzYW24Bya4I9/cAyW/RnwmU9pQww==";

        private static readonly Lazy<ScannerModel> instance = new Lazy<ScannerModel>(() => new ScannerModel(), LazyThreadSafetyMode.PublicationOnly);

        public static ScannerModel Instance => instance.Value;
        private ScannerModel()
        {

            this.CurrentCamera?.ApplySettingsAsync(this.CameraSettings);
            this.DataCaptureContext = DataCaptureContext.ForLicenseKey(SCANDIT_LICENSE_KEY);
            this.DataCaptureContext.SetFrameSourceAsync(this.CurrentCamera);
            this.BarcodeCaptureSettings = BarcodeCaptureSettings.Create();
            HashSet<Symbology> symbologies = new HashSet<Symbology>
            {
                //Symbology.Ean13Upca,
                //Symbology.Ean8,
                //Symbology.Upce,
                //Symbology.Qr,
                //Symbology.DataMatrix,
               // Symbology.Code39,
                Symbology.Code128,
               // Symbology.InterleavedTwoOfFive
            };

            this.BarcodeCaptureSettings.EnableSymbologies(symbologies);
            TimeSpan interval = TimeSpan.FromMilliseconds(1500);
            this.BarcodeCaptureSettings.CodeDuplicateFilter = interval;
            this.BarcodeCapture = BarcodeCapture.Create(this.DataCaptureContext, this.BarcodeCaptureSettings);
        }


        #region DataCaptureContext
        public DataCaptureContext DataCaptureContext { get; private set; }
        #endregion

        #region CamerSettings
        public Camera CurrentCamera { get; private set; } = Camera.GetCamera(CameraPosition.WorldFacing);
        public CameraSettings CameraSettings { get; } = BarcodeCapture.RecommendedCameraSettings;
        #endregion
     
        #region BarcodeCapture
        public BarcodeCapture BarcodeCapture { get; private set; }
        public BarcodeCaptureSettings BarcodeCaptureSettings { get; private set; }
        #endregion

        #region ScanArea Settings
        public MarginsWithUnit ScanAreaMargins { get; set; } = new MarginsWithUnit(
            new FloatWithUnit(0, MeasureUnit.Dip),
            new FloatWithUnit(300, MeasureUnit.Dip),
            new FloatWithUnit(0, MeasureUnit.Dip),
            new FloatWithUnit(300, MeasureUnit.Dip)
        );
        #endregion
    }
}
