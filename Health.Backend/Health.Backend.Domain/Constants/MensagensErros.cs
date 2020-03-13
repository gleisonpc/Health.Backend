namespace Health.Backend.Domain.Constants
{
    public static class MensagensErros
    {
        public const string SEGURADO_COM_MENOS_DE_18_ANOS = "Segurado com menos de 18 anos não são permitidos.";
        public const string CIDADE_NAO_ENCONTRADA = "Cidade não autorizada para cotacao.";
        public const string CEP_FORA_DO_PADRA_PERMITIDO = "CEP fora do padrão permitido.";
        public const string SEGURADO_SEM_NENHUMA_COBERTURA_OBRIGATORIA = "Segurado sem nenhuma cobertura obrigatória.";
    }
}
