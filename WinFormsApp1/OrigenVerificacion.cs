namespace CapaVista
{
    // Contexto desde el que se abren frmCodigo/frmPreguntas: define qué pasa
    // después de verificar correctamente al usuario (código y/o preguntas).
    public enum OrigenVerificacion
    {
        // Recuperar cuenta desde frmLogin: al verificar, se genera y envía
        // una contraseña temporal por correo (no se deja elegir una nueva acá).
        RecuperarContrasena,

        // Cambio de contraseña voluntario desde frmUserConfig (usuario ya logueado):
        // al verificar, se lo deja elegir la nueva contraseña directamente.
        CambioVoluntario,

        // Confirmación de 2FA durante el login estándar (frmLogin, credenciales ya
        // correctas): al verificar el código, se entra directo a la pantalla principal.
        ConfirmacionLogin
    }
}
