namespace Modelo.Aplicacao.Enums
{ 

    public enum TipoConsultaPagamentos
    {
        Inadimplentes = 1,//Inadimplentes: As informações das parcelas que não foram quitadas após 60 dias.
        ParcelasAVencer = 2,//Parcelas a vencer: As informações das parcelas que irão vencer.
        ParcelasBaixadas = 3,//Parcelas baixadas: As informações das parcelas que foram baixadas pelo Banco do Brasil ou pelo sistema de cobrança.
        ParcelasLiquidadas = 4,//As informações das parcelas que foram liquidadas/pagas pelo empreendimento.
        ParcelasVencidasOuNaoPagas = 5//Parcelas vencidas ou não pagas: As informações das parcelas que estão vencidas ou que não foram pagas.
    }

}
