using Utilidades;

namespace CapaLogica
{
    public class CL_servicioMail
    {
        public bool enviarContraTemporal(string correo, string passwordTemporal)
        {
            if (string.IsNullOrWhiteSpace(correo))
                return false;

            if (string.IsNullOrWhiteSpace(passwordTemporal))
                return false;

            string asunto = "Contraseña temporal del sistema";

            string mensaje = $@"
<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
    <title>Recuperación de cuenta</title>
    <style>
        body {{
            margin: 0;
            padding: 0;
            background-color: #f4f4f4;
            font-family: Arial, Helvetica, sans-serif;
            color: #444444;
        }}

        .wrapper {{
            width: 100%;
            padding: 30px 0;
        }}

        .container {{
            max-width: 600px;
            margin: 0 auto;
            background-color: #ffffff;
            border-radius: 8px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.08);
            padding: 30px 40px;
        }}

        .logo {{
            text-align: center;
            margin-bottom: 10px;
        }}

        .logo img {{
            width: 50px;
            height: 50px;
            border-radius: 50%;
        }}

        .title {{
            text-align: center;
            font-size: 22px;
            font-weight: 700;
            margin: 10px 0 20px;
            color: #222222;
        }}

        .text {{
            font-size: 14px;
            line-height: 22px;
            margin-bottom: 16px;
        }}

        .code-box {{
            background-color: #f7f7f7;
            border-radius: 6px;
            padding: 15px;
            text-align: center;
            margin: 20px 0;
        }}

        .code-label {{
            font-size: 12px;
            text-transform: uppercase;
            letter-spacing: 1px;
            color: #777777;
            margin-bottom: 6px;
        }}

        .code-value {{
            font-size: 22px;
            font-weight: bold;
            color: #2ba612;
        }}

        .footer {{
            font-size: 11px;
            color: #999999;
            text-align: center;
            margin-top: 20px;
        }}
    </style>
</head>

<body>
    <div class='wrapper'>
        <div class='container'>

            <div class='logo'>
                <img src='logo.jpg' alt='Logo'>
            </div>

            <div class='title'>Se solicitó una nueva contraseña</div>

            <p class='text'>
                Hola, se generó una contraseña temporal para tu cuenta.
                Utilizala para iniciar sesión y, por seguridad, cambiala ni bien entres al sistema.
            </p>

            <div class='code-box'>
                <div class='code-label'>Tu contraseña temporal</div>
                <div class='code-value'>{passwordTemporal}</div>
            </div>

            <p class='text'>
                Si no fuiste vos quien solicitó este cambio, te recomendamos cambiar la contraseña
                nuevamente y revisar la actividad de tu cuenta.
            </p>

            <div class='footer'>
                Este mensaje fue enviado automáticamente. Por favor, no respondas este correo.
            </div>

        </div>
    </div>
</body>
</html>";

            try
            {
                enviarMail.sendMail(correo, asunto, mensaje);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EnviarCodigoRecuperacion(string correo, string codigo)
        {
            if (string.IsNullOrWhiteSpace(correo))
                return false;

            if (string.IsNullOrWhiteSpace(codigo))
                return false;

            string asunto = "Código de recuperación";

            string mensaje = $@"
<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
    <title>Código de recuperación</title>
    <style>
        body {{
            margin: 0;
            padding: 0;
            background-color: #f4f4f4;
            font-family: Arial, Helvetica, sans-serif;
            color: #444444;
        }}

        .wrapper {{
            width: 100%;
            padding: 30px 0;
        }}

        .container {{
            max-width: 600px;
            margin: 0 auto;
            background-color: #ffffff;
            border-radius: 8px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.08);
            padding: 30px 40px;
        }}

        .logo {{
            text-align: center;
            margin-bottom: 10px;
        }}

        .logo img {{
            width: 50px;
            height: 50px;
            border-radius: 50%;
        }}

        .title {{
            text-align: center;
            font-size: 22px;
            font-weight: 700;
            margin: 10px 0 20px;
            color: #222222;
        }}

        .text {{
            font-size: 14px;
            line-height: 22px;
            margin-bottom: 16px;
        }}

        .code-box {{
            background-color: #f7f7f7;
            border-radius: 6px;
            padding: 15px;
            text-align: center;
            margin: 20px 0;
        }}

        .code-label {{
            font-size: 12px;
            text-transform: uppercase;
            letter-spacing: 1px;
            color: #777777;
            margin-bottom: 6px;
        }}

        .code-value {{
            font-size: 22px;
            font-weight: bold;
            color: #2ba612;
        }}

        .footer {{
            font-size: 11px;
            color: #999999;
            text-align: center;
            margin-top: 20px;
        }}
    </style>
</head>

<body>
    <div class='wrapper'>
        <div class='container'>

            <div class='logo'>
                <img src='logo.jpg' alt='Logo'>
            </div>

            <div class='title'>Código de recuperación</div>

            <p class='text'>
                Recibimos una solicitud para recuperar el acceso a tu cuenta.
                Usá el siguiente código para continuar con el proceso.
            </p>

            <div class='code-box'>
                <div class='code-label'>Tu código</div>
                <div class='code-value'>{codigo}</div>
            </div>

            <p class='text'>
                Si no solicitaste este código, podés ignorar este mensaje.
            </p>

            <div class='footer'>
                Este mensaje fue enviado automáticamente. Por favor, no respondas este correo.
            </div>

        </div>
    </div>
</body>
</html>";

            try
            {
                enviarMail.sendMail(correo, asunto, mensaje);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}