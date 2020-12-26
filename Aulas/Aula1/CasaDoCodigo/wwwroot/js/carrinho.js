
class Carrinho {

    clickIncremento(button) {

        let data = this.getData(button);
        data.Quantidade++;
        this.postQuantidade(data);
    }

    clickDecremento(button) {
        let data = this.getData(button);
        data.Quantidade--;
        this.postQuantidade(data);
    }

    getData(elementoHTML) {
        //Atraves do parents buscamos algum elemento dos pais daquele item.
        //ou seja localizar a cima.
        var linhaItem = $(elementoHTML).parents('[item-id]');

        //Atraves do '$' acessamos os metodos do Jquery
        var itemId = $(linhaItem).attr('item-id');//Com attr é possivel recuperar um atributo do elemento

        //Atravez do find buscamos algum alemto filho, ou seja localizar a baixo
        var novaQuantidade = $(linhaItem).find('input').val();

        return {
            id: itemId,
            Quantidade: novaQuantidade
        };
    }

    postQuantidade(data) {

        let token = $('[name=__RequestVerificationToken]').val();

        let headers = {};
        headers['RequestVerificationToken'] = token;

        $.ajax({
            url: '/pedido/updatequantidade',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            headers: headers
        }).done(function (response) {//Metodo que recebe a resposta da requisição
            //location.reload();//Atualiza a Pagina

            let itemPedido = response.itemPedido;
            let linhaDoitem = $('[item-id=' + itemPedido.id + ']');
            linhaDoitem.find('input').val(itemPedido.quantidade);
            linhaDoitem.find('[subtotal]').html((itemPedido.subtotal).duasCasas());

            let carrinhoViewModel = response.carrinhoViewModel;
            $('[numero-itens]').html('Total:' + carrinhoViewModel.itens.length + ' itens')
            $('[total]').html((carrinhoViewModel.total).duasCasas());

            if (itemPedido.quantidade == 0) {
                linhaDoitem.remove();
            }
        });
    }

    updateQuantidade(input) {
        let data = this.getData(input);
        this.postQuantidade(data);
    }
}

var carrinho = new Carrinho();

Number.prototype.duasCasas = function () {
    return this.toFixed(2).replace('.', ',');
}

