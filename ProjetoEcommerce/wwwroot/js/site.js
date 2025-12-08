/* ============================
   CARRINHO (LOCALSTORAGE)
============================ */

let carrinho = JSON.parse(localStorage.getItem("carrinho")) || [];

// ---------- Salvar carrinho ----------
function salvarCarrinho() {
    localStorage.setItem("carrinho", JSON.stringify(carrinho));
}

// ---------- Atualizar carrinho ----------
function atualizarCarrinho() {
    const lista = document.getElementById("lista-carrinho");
    const totalDiv = document.getElementById("total-carrinho");

    if (!lista || !totalDiv) return;

    lista.innerHTML = "";
    let total = 0;

    carrinho.forEach((item, index) => {
        total += item.preco;

        lista.innerHTML += `
            <div class="item-carrinho">
                <span>${item.nome}</span>
                <span>R$ ${item.preco.toFixed(2)}</span>
                <button class="remover" onclick="removerItem(${index})">x</button>
            </div>
        `;
    });

    totalDiv.innerHTML = `Total: R$ ${total.toFixed(2)}`;
}

// ---------- Abrir / Fechar popup ----------
function abrirCarrinho() {
    document.getElementById("popup-carrinho").style.display = "block";
    atualizarCarrinho();
}

function fecharCarrinho() {
    document.getElementById("popup-carrinho").style.display = "none";
}

// ---------- Remover item ----------
function removerItem(i) {
    carrinho.splice(i, 1);
    salvarCarrinho();
    atualizarCarrinho();
}

// ---------- FINALIZAR ----------
function finalizarPedido() {
    if (carrinho.length === 0) {
        alert("Sua sacola está vazia!");
        return;
    }
    alert("Pedido finalizado com sucesso!");
    carrinho = [];
    salvarCarrinho();
    atualizarCarrinho();
}

/* ============================
   ADICIONAR AO CARRINHO
============================ */

// Recebe dados do servidor (injetado no Layout)
let usuarioLogado = window.usuarioEstaLogado === true;

function adicionarAoCarrinho(nome, preco) {

    if (!usuarioLogado) {
        alert("Você precisa fazer login para adicionar itens à sacola!");
        return;
    }

    carrinho.push({ nome, preco });
    salvarCarrinho();
    atualizarCarrinho();

    abrirCarrinho();
}

// Inicializa carrinho ao carregar
document.addEventListener("DOMContentLoaded", atualizarCarrinho);
