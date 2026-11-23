namespace CapaVista
{
    // desde donde se abre frmcodigo o frmpreguntas, define que pasa
    // despues de verificar bien al usuario (codigo y/o preguntas)
    public enum OrigenVerificacion
    {
        // recuperar cuenta desde frmlogin: al verificar se genera y se manda
        // una contraseña temporal por mail (aca no se deja elegir una nueva)
        RecuperarContrasena,

        // cambio de contraseña desde frmuserconfig (el usuario ya esta logueado):
        // al verificar se lo deja elegir la contraseña nueva directo
        CambioVoluntario,

        // confirmar el 2fa en el login normal (frmlogin, usuario y contraseña ya
        // correctos): al verificar el codigo entra directo a la pantalla principal
        ConfirmacionLogin
    }
}
