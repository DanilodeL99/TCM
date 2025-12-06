// wwwroot/js/carrinho.js
(function () {
    const popup = document.getElementById('popup-carrinho');
    const btnOpen = document.getElementById('btnOpenCart');
    const btnClose = document.getElementById('btnFecharCarrinho');
    const listaEl = document.getElementById('lista-carrinho');
    const totalEl = document.getElementById('total-carrinho');
    const btnClear = document.getElementById('btnClearCart');
    const btnFinalizar = document.getElementById('btnFinalizar');

    // abrir / fechar
    function abrirCarrinho() {
        loadCart();
        popup.style.display = 'block';
        popup.setAttribute('aria-hidden', 'false');
    }
    function fecharCarrinho() {
        popup.style.display = 'none';
        popup.setAttribute('aria-hidden', 'true');
    }

    btnOpen && btnOpen.addEventListener('click', abrirCarrinho);
    btnClose && btnClose.addEventListener('click', fecharCarrinho);

    // carregar carrinho do servidor
    async function loadCart() {
        listaEl.innerHTML = '<div class="lista-vazia">Carregando...</div>';
        try {
            const res = await fetch('/Carrinho/Listar');
            const data = await res.json();

            renderCart(data || []);
        } catch (err) {
            listaEl.innerHTML = '<div class="lista-vazia">Erro ao carregar.</div>';
            console.error(err);
        }
    }

    // renderizar itens
    function renderCart(items) {
        if (!items || items.length === 0) {
            listaEl.innerHTML = '<div class="lista-vazia">Seu carrinho está vazio.</div>';
            totalEl.textContent = 'Total: R$ 0,00';
            return;
        }

        listaEl.innerHTML = '';
        let total = 0;
        items.forEach(it => {
            total += (it.preco || it.Preco || it.PrecoProd || 0) * (it.quantidade || it.Quantidade || it.Quantidade || it.QuantidadeItem || 1);

            const preco = (it.preco ?? it.Preco ?? it.PrecoProd ?? 0).toFixed(2);

            const div = document.createElement('div');
            div.className = 'carrinho-item';
            div.innerHTML = `
                <img src="${it.imagem ?? it.Imagem ?? it.imagemUrl ?? '/img/placeholder.png'}" />
                <div class="carrinho-info">
                    <div class="carrinho-nome">${it.nome ?? it.Nome ?? ''}</div>
                    <div class="carrinho-preco">R$ ${preco}</div>
                    <div class="qty-control">
                        <button class="qty-btn btn-decrease" data-id="${it.produtoId ?? it.ProdutoId ?? it.ProdutoId}">-</button>
                        <div class="qty-value">${it.quantidade ?? it.Quantidade ?? 1}</div>
                        <button class="qty-btn btn-increase" data-id="${it.produtoId ?? it.ProdutoId ?? it.ProdutoId}">+</button>
                        <button class="remove-btn" data-id="${it.produtoId ?? it.ProdutoId ?? it.ProdutoId}">Excluir</button>
                    </div>
                </div>
            `;
            listaEl.appendChild(div);
        });

        totalEl.textContent = 'Total: R$ ' + total.toFixed(2);

        // ligações dos botões
        document.querySelectorAll('.btn-increase').forEach(b => b.addEventListener('click', async (e) => {
            const id = parseInt(e.currentTarget.dataset.id);
            await changeQuantity(id, 1);
        }));

        document.querySelectorAll('.btn-decrease').forEach(b => b.addEventListener('click', async (e) => {
            const id = parseInt(e.currentTarget.dataset.id);
            await changeQuantity(id, -1);
        }));

        document.querySelectorAll('.remove-btn').forEach(b => b.addEventListener('click', async (e) => {
            const id = parseInt(e.currentTarget.dataset.id);
            await removeItem(id);
        }));
    }

    // increase/decrease by +/-1
    async function changeQuantity(produtoId, delta) {
        try {
            // primeiro pega lista atual para calcular nova qtd
            const res = await fetch('/Carrinho/Listar');
            const items = await res.json();
            const item = items.find(x => (x.produtoId ?? x.ProdutoId) == produtoId);
            if (!item) return;
            const current = parseInt(item.quantidade ?? item.Quantidade ?? 1);
            const nova = current + delta;
            if (nova <= 0) {
                // remover
                await fetch('/Carrinho/Remover', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ ProdutoId: produtoId })
                });
            } else {
                await fetch('/Carrinho/AtualizarQuantidade', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ ProdutoId: produtoId, Quantidade: nova })
                });
            }
            await loadCart();
        } catch (err) { console.error(err); }
    }

    // remover
    async function removeItem(produtoId) {
        try {
            await fetch('/Carrinho/Remover', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ ProdutoId: produtoId })
            });
            await loadCart();
        } catch (err) { console.error(err); }
    }

    // limpar carrinho local (pede ao servidor para salvar vazio)
    btnClear && btnClear.addEventListener('click', async function () {
        try {
            // enviar remoção para cada item
            const res = await fetch('/Carrinho/Listar');
            const items = await res.json();
            for (const it of items) {
                const id = it.produtoId ?? it.ProdutoId;
                await fetch('/Carrinho/Remover', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ ProdutoId: id })
                });
            }
            await loadCart();
        } catch (err) { console.error(err); }
    });

    btnFinalizar && btnFinalizar.addEventListener('click', function () {
        alert('Implementar fluxo de finalização (checkout) conforme sua regra de negócio.');
    });

    // =================================================================
    // binding dos botões "Adicionar ao Pedido" nas páginas de produto
    // cada .prato-item deve ter: data-id, data-nome, data-preco, data-img
    // =================================================================
    function bindAddButtons() {
        document.querySelectorAll('.prato-item .botao-pedido').forEach(btn => {
            btn.addEventListener('click', async function (e) {
                const el = e.currentTarget.closest('.prato-item');
                if (!el) return;

                const produtoId = parseInt(el.dataset.id || el.dataset.cod || el.dataset.produtoid || 0);
                const nome = el.dataset.nome || el.querySelector('.prato-nome')?.innerText || '';
                const preco = parseFloat(el.dataset.preco || el.dataset.price || el.querySelector('.prato-preco')?.innerText.replace(/[^\d,.-]/g, '').replace(',', '.') || 0);
                const img = el.dataset.img || el.querySelector('img')?.getAttribute('src') || '/img/placeholder.png';

                const payload = {
                    ProdutoId: produtoId,
                    Nome: nome,
                    Preco: preco,
                    Quantidade: 1,
                    Imagem: img
                };

                try {
                    await fetch('/Carrinho/Adicionar', {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify(payload)
                    });
                    // abrir e recarregar
                    abrirCarrinho();
                } catch (err) {
                    console.error(err);
                }
            });
        });
    }

    // ligar quando DOM pronto
    document.addEventListener('DOMContentLoaded', function () {
        bindAddButtons();
        // pré-carrega para evitar delay quando abrir
        // loadCart();
    });

    // exporta funções globais (se precisar)
    window.abrirCarrinho = abrirCarrinho;
    window.fecharCarrinho = fecharCarrinho;
})();
