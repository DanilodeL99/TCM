/* carrinho.js - versão robusta para interação localStorage, quantidades, remover e finalizar */

/* key do localStorage */
const CART_KEY = "carrinhoPitaya_v1";

/* estado em memória */
let carrinho = [];

/* Helpers */
function salvarCarrinho() {
    localStorage.setItem(CART_KEY, JSON.stringify(carrinho));
}
function carregarCarrinho() {
    const raw = localStorage.getItem(CART_KEY);
    carrinho = raw ? JSON.parse(raw) : [];
}

/* Atualiza exibição do popup (lista + total) */
function atualizarExibicaoCarrinho() {
    const lista = document.getElementById("lista-carrinho");
    const totalEl = document.getElementById("total-carrinho");
    if (!lista || !totalEl) return;

    lista.innerHTML = "";

    if (carrinho.length === 0) {
        lista.innerHTML = `<div class="lista-vazia">Seu carrinho está vazio.</div>`;
        totalEl.textContent = "Total: R$ 0,00";
        return;
    }

    let total = 0;

    carrinho.forEach((item, idx) => {
        total += item.preco * item.qtd;

        const itemHtml = document.createElement("div");
        itemHtml.className = "item-carrinho";

        const img = document.createElement("img");
        img.className = "img-carrinho";
        img.src = item.imagem || "/img/Logo.png";
        img.alt = item.nome;

        const info = document.createElement("div");
        info.className = "info-carrinho";

        const nome = document.createElement("div");
        nome.className = "nome-item";
        nome.textContent = item.nome;

        const preco = document.createElement("div");
        preco.className = "preco-item";
        preco.textContent = `R$ ${Number(item.preco).toFixed(2).replace('.', ',')}`;

        const qtdCtrl = document.createElement("div");
        qtdCtrl.className = "qtd-controls";

        const btnMinus = document.createElement("button");
        btnMinus.className = "btn btn-sm btn-outline-secondary";
        btnMinus.textContent = "-";
        btnMinus.onclick = () => {
            alterarQuantidade(idx, item.qtd - 1);
        };

        const spanQtd = document.createElement("span");
        spanQtd.style.margin = "0 8px";
        spanQtd.textContent = item.qtd;

        const btnPlus = document.createElement("button");
        btnPlus.className = "btn btn-sm btn-outline-secondary";
        btnPlus.textContent = "+";
        btnPlus.onclick = () => {
            alterarQuantidade(idx, item.qtd + 1);
        };

        qtdCtrl.appendChild(btnMinus);
        qtdCtrl.appendChild(spanQtd);
        qtdCtrl.appendChild(btnPlus);

        const remover = document.createElement("button");
        remover.className = "btn-remover";
        remover.textContent = "Remover";
        remover.onclick = () => {
            removerItem(idx);
        };

        info.appendChild(nome);
        info.appendChild(preco);
        info.appendChild(qtdCtrl);

        itemHtml.appendChild(img);
        itemHtml.appendChild(info);
        itemHtml.appendChild(remover);

        lista.appendChild(itemHtml);
    });

    totalEl.textContent = `Total: R$ ${total.toFixed(2).replace('.', ',')}`;
}

/* Adiciona item: id (string), nome, preco (number), imagem (url opcional) */
function addToCart(id, nome, preco, imagem) {
    // garante boolean JS correto (layout define window.usuarioEstaLogado)
    if (!window.usuarioEstaLogado) {
        alert("Você precisa fazer login para adicionar itens à sacola.");
        return;
    }

    // buscar item existente
    const existente = carrinho.find(x => x.id === id);
    if (existente) {
        existente.qtd += 1;
    } else {
        carrinho.push({ id, nome, preco: Number(preco), imagem: imagem || "/img/Logo.png", qtd: 1 });
    }

    salvarCarrinho();
    atualizarExibicaoCarrinho();
}

/* alterar quantidade por índice no array */
function alterarQuantidade(index, novaQtd) {
    if (novaQtd <= 0) {
        removerItem(index);
        return;
    }
    if (carrinho[index]) {
        carrinho[index].qtd = novaQtd;
        salvarCarrinho();
        atualizarExibicaoCarrinho();
    }
}

/* remover item por índice */
function removerItem(index) {
    if (index >= 0 && index < carrinho.length) {
        carrinho.splice(index, 1);
        salvarCarrinho();
        atualizarExibicaoCarrinho();
    }
}

/* limpar */
function limparCarrinho() {
    carrinho = [];
    salvarCarrinho();
    atualizarExibicaoCarrinho();
}

/* abrir / fechar (expostos globalmente para layout) */
function abrirCarrinho() {
    const popup = document.getElementById("popup-carrinho");
    if (!popup) return;
    carregarCarrinho();
    atualizarExibicaoCarrinho();
    popup.style.display = "block";
}

function fecharCarrinho() {
    const popup = document.getElementById("popup-carrinho");
    if (!popup) return;
    popup.style.display = "none";
}

/* finalizar: redireciona só se houver itens */
function finalizarCompra() {
    carregarCarrinho();
    if (!carrinho || carrinho.length === 0) {
        alert("Seu carrinho está vazio. Adicione itens antes de finalizar.");
        return;
    }
    // redireciona para a página de pagamento (Privacy)
    window.location.href = "/Home/Privacy";
}

/* liga botões e carrega estado ao iniciar */
document.addEventListener("DOMContentLoaded", function () {
    carregarCarrinho();
    atualizarExibicaoCarrinho();

    const btnOpen = document.getElementById("btnOpenCart");
    const btnClose = document.getElementById("btnFecharCarrinho");
    const btnClear = document.getElementById("btnClearCart");
    const btnFinal = document.getElementById("btnFinalizar");

    if (btnOpen) btnOpen.addEventListener("click", abrirCarrinho);
    if (btnClose) btnClose.addEventListener("click", fecharCarrinho);
    if (btnClear) btnClear.addEventListener("click", function () { if (confirm("Limpar sacola?")) limparCarrinho(); });
    if (btnFinal) btnFinal.addEventListener("click", finalizarCompra);

    // expõe funções globais necessárias (para chamadas diretas)
    window.addToCart = addToCart;
    window.abrirCarrinho = abrirCarrinho;
    window.fecharCarrinho = fecharCarrinho;
    window.finalizarCompra = finalizarCompra;
});
