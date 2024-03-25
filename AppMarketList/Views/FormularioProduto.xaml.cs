using AppMarketList.Models;

namespace AppMarketList.Views;

public partial class FormularioProduto : ContentPage
{
	public FormularioProduto()
	{
		InitializeComponent();
        if(BindingContext == "Cria��o de produto")
        { 
            txt_descricao.Text = String.Empty;
            txt_quantidade.Text = String.Empty;
            txt_preco.Text = String.Empty;
        }
        
        
	}

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        //Cria��o de produto
        if (BindingContext == "Cria��o de produto")
        {
            try
            {
                Produto p = new Produto
                {
                    Descricao = txt_descricao.Text,
                    Quantidade = Convert.ToDouble(txt_quantidade.Text),
                    Preco = Convert.ToDouble(txt_preco.Text),
                };

                await App.Db.Insert(p);
                await DisplayAlert("Sucesso!", "Produto criado com sucesso", "Ok");
                await Navigation.PushAsync(new MainPage());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "Fechar");
            }


        }
        else
        {
            //Edi��o de produto

            try
            {
                Produto produto_anexado = BindingContext as Produto;

                Produto p = new Produto
                {
                    Id = produto_anexado.Id,
                    Descricao = txt_descricao.Text,
                    Quantidade = Convert.ToDouble(txt_quantidade.Text),
                    Preco = Convert.ToDouble(txt_preco.Text),
                };

                await App.Db.Update(p);
                await DisplayAlert("Sucesso!", "Produto alterado com sucesso", "Ok");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "Fechar");
            }
        }
    }
}