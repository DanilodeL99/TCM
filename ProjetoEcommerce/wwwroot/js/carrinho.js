async function abrirCarrinho() {
    document.getElementById("popup-carrinho").classList.add("ativo");
    await carregarCarrinho();
}

function fecharCarrinho() {
    document.getElementById("popup-carrinho").classList.remove("ativo");
}

async function carregarCarrinho() {
    const resp = await fetch('/Carrinho/Listar');
    const itens = await resp.json();

    const lista = document.getElementById('lista-carrinho');
    lista.innerHTML = '';

    let total = 0;
    if (!itens || itens.length === 0) {
        lista.innerHTML = '<p>Seu carrinho está vazio.</p>';
    } else {
        itens.forEach(i => {
            total += (i.preco * i.quantidade);
            const el = document.createElement('div');
            el.className = 'item-carrinho';
            el.innerHTML = `
                <img src="${i.imagem}" />
                <div class="meta">
                    <div class="nome">${i.nome}</div>
                    <div class="preco">R$ ${i.preco.toFixed(2)}</div>
                </div>
                <div class="acoes">
                    <button class="btn-qty" onclick="atualizarQtd(${i.produtoId}, ${i.quantidade - 1})">-</button>
                    <div>${i.quantidade}</div>
                    <button class="btn-qty" onclick="atualizarQtd(${i.produtoId}, ${i.quantidade + 1})">+</button>
                    <button class="btn-small btn-danger" onclick="remover(${i.produtoId})">Excluir</button>
                </div>
            `;
            lista.appendChild(el);
        });
    }

    document.getElementById('total-carrinho').innerText = `Total: R$ ${total.toFixed(2)}`;
}

async function adicionarCarrinho(produtoId, nome, preco, imagem, quantidade = 1) {
    const body = { produtoId: produtoId, nome: nome, preco: preco, imagem: imagem, quantidade: quantidade };
    await fetch('/Carrinho/Adicionar', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body)
    });
    abrirCarrinho(); // abre e recarrega
}

async function atualizarQtd(produtoId, quantidade) {
    if (quantidade <= 0) {
        await remover(produtoId);
        return;
    }
    await fetch('/Carrinho/AtualizarQuantidade', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ produtoId, quantidade })
    });
    carregarCarrinho();
}

async function remover(produtoId) {
    await fetch('/Carrinho/Remover', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ produtoId })
    });
    carregarCarrinho();
}

function finalizarPedido() {
    alert('Funcionalidade de finalizar pedido a implementar (ex: gravar pedido no BD).');
}
