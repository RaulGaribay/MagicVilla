namespace MagicVilla_API.Modelos
{
    public class Usuario
    {
        //Cuando se le pone a una propiedad el nombre "Id", Entity Framework Core lo identifica como el
        //parámetro key, por lo que no es necesario especificarlo. Por alguna razón no explicó eso desde
        //el inicio y en la otras clases (NumeroVilla y Villa) sí se especificó que Id era la llave primaria.
        //Esto es solo para cuando se usan migraciones.
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Nombre { get; set; }

        public string Password { get; set; }

        public string Rol { get; set; }
    }
}
