document.addEventListener("DOMContentLoaded", function () {

    const popup = document.getElementById("popup-carrinho");
    const btnOpenCart = document.getElementById("btnOpenCart");
    const btnClose = document.getElementById("btnFecharCarrinho");

    const listaCarrinho = document.getElementById("lista-carrinho");
    const totalCarrinho = document.getElementById("total-carrinho");
    const btnClear = document.getElementById("btnClearCart");
    const btnFinalizar = document.getElementById("btnFinalizar");

    let carrinho = JSON.parse(localStorage.getItem("carrinhoPV")) || [];

    function salvarCarrinho() {
        localStorage.setItem("carrinhoPV", JSON.stringify(carrinho));
    }

    function atualizarCarrinho() {
        listaCarrinho.innerHTML = "";

        if (carrinho.length === 0) {
            listaCarrinho.innerHTML = `<div class="lista-vazia">Sua sacola está vazia.</div>`;
            totalCarrinho.textContent = "Total: R$ 0,00";
            return;
        }

        let total = 0;

        carrinho.forEach((item, index) => {
            total += item.preco * item.qtd;

            const linha = document.createElement("div");
            linha.classList.add("item-carrinho");

            linha.innerHTML = `
                <span>${item.nome}</span>
                <span>R$ ${item.preco.toFixed(2).replace(".", ",")}</span>
                <span>Qtd: ${item.qtd}</span>
                <button class="btn btn-sm btn-danger" data-index="${index}">X</button>
            `;

            listaCarrinho.appendChild(linha);
        });

        totalCarrinho.textContent = "Total: R$ " + total.toFixed(2).replace(".", ",");

        document.querySelectorAll(".btn-danger").forEach(btn => {
            btn.addEventListener("click", function () {
                const index = this.getAttribute("data-index");
                carrinho.splice(index, 1);
                salvarCarrinho();
                atualizarCarrinho();
            });
        });
    }

    if (btnOpenCart) {
        btnOpenCart.addEventListener("click", () => {
            popup.style.display = "block";
            atualizarCarrinho();
        });
    }

    if (btnClose) {
        btnClose.addEventListener("click", () => {
            popup.style.display = "none";
        });
    }

    if (btnClear) {
        btnClear.addEventListener("click", () => {
            carrinho = [];
            salvarCarrinho();
            atualizarCarrinho();
        });
    }

    if (btnFinalizar) {
        btnFinalizar.addEventListener("click", () => {

            if (carrinho.length === 0) {
                alert("Sua sacola está vazia!");
                return;
            }

            window.location.href = "/Home/Privacy";
        });
    }
});

// FUNÇÃO GLOBAL USADA PELOS BOTÕES DO CARDÁPIO
window.addToCart = function (nome, preco) {

    if (window.usuarioEstaLogado !== "true") {
        alert("Você precisa fazer login para adicionar um item à sacola!");
        return;
    }

    let carrinho = JSON.parse(localStorage.getItem("carrinhoPV")) || [];

    const itemExistente = carrinho.find(x => x.nome === nome);

    if (itemExistente) {
        itemExistente.qtd++;
    } else {
        carrinho.push({ nome, preco, qtd: 1 });
    }

    localStorage.setItem("carrinhoPV", JSON.stringify(carrinho));
    alert("Item adicionado à sacola!");
};
