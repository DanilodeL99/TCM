(function () {
    const popup = document.getElementById('popup-carrinho');
    const btnOpen = document.getElementById('btnOpenCart');
    const btnClose = document.getElementById('btnFecharCarrinho');
    const listaEl = document.getElementById('lista-carrinho');
    const totalEl = document.getElementById('total-carrinho');
    const btnClear = document.getElementById('btnClearCart');
    const btnFinalizar = document.getElementById('btnFinalizar');

    function abrirCarrinho() {
        loadCart();
        popup.style.display = 'block';
    }
    function fecharCarrinho() {
        popup.style.display = 'none';
    }

    btnOpen && btnOpen.addEventListener('click', abrirCarrinho);
    btnClose && btnClose.addEventListener('click', fecharCarrinho);

    async function loadCart() {
        listaEl.innerHTML = '<div class="lista-vazia">Carregando...</div>';
        try {
            const res = await fetch('/Carrinho/Listar');
            if (!res.ok) { listaEl.innerHTML = '<div class="lista-vazia">Erro ao carregar.</div>'; return; }
            const data = await res.json();
            renderCart(data || []);
        } catch (err) {
            listaEl.innerHTML = '<div class="lista-vazia">Erro ao carregar.</div>';
            console.error(err);
        }
    }

    function renderCart(items) {
        if (!items || items.length === 0) {
            listaEl.innerHTML = '<div class="lista-vazia">Seu carrinho está vazio.</div>';
            totalEl.textContent = 'Total: R$ 0,00';
            return;
        }

        listaEl.innerHTML = '';
        let total = 0;
        items.forEach(it => {
            total += (it.preco ?? it.Preco ?? it.PrecoProd ?? 0) * (it.quantidade ?? it.Quantidade ?? 1);
            const preco = (it.preco ?? it.Preco ?? it.PrecoProd ?? 0).toFixed(2);

            const div = document.createElement('div');
            div.className = 'carrinho-item';
            div.innerHTML = `
                <img src="${it.imagem ?? it.Imagem ?? '/img/placeholder.png'}" />
                <div class="carrinho-info">
                    <div class="carrinho-nome">${it.nome ?? it.Nome ?? ''}</div>
                    <div class="carrinho-preco">R$ ${preco}</div>
                    <div class="qty-control">
                        <button class="qty-btn btn-decrease" data-id="${it.produtoId ?? it.ProdutoId}">-</button>
                        <div class="qty-value">${it.quantidade ?? it.Quantidade ?? 1}</div>
                        <button class="qty-btn btn-increase" data-id="${it.produtoId ?? it.ProdutoId}">+</button>
                        <button class="remove-btn" data-id="${it.produtoId ?? it.ProdutoId}">Excluir</button>
                    </div>
                </div>
            `;
            listaEl.appendChild(div);
        });

        totalEl.textContent = 'Total: R$ ' + total.toFixed(2);

        document.querySelectorAll('.btn-increase').forEach(b => b.addEventListener('click', e => changeQuantity(parseInt(e.currentTarget.dataset.id), +1)));
        document.querySelectorAll('.btn-decrease').forEach(b => b.addEventListener('click', e => changeQuantity(parseInt(e.currentTarget.dataset.id), -1)));
        document.querySelectorAll('.remove-btn').forEach(b => b.addEventListener('click', e => removeItem(parseInt(e.currentTarget.dataset.id))));
    }

    async function changeQuantity(produtoId, delta) {
        try {
            const res = await fetch('/Carrinho/Listar');
            const items = await res.json();
            const item = items.find(x => (x.produtoId ?? x.ProdutoId) == produtoId);
            if (!item) return;
            const current = parseInt(item.quantidade ?? item.Quantidade ?? 1);
            const nova = current + delta;
            if (nova <= 0) {
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

    btnClear && btnClear.addEventListener('click', async function () {
        try {
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
        alert('Fluxo de finalização a implementar (checkout).');
    });

    // adicionar produto - exige autenticação (servidor verifica sessão)
    function bindAddButtons() {
        document.querySelectorAll('.prato-item .botao-pedido').forEach(btn => {
            btn.addEventListener('click', async function (e) {
                const el = e.currentTarget.closest('.prato-item');
                if (!el) return;

                const produtoId = parseInt(el.dataset.id || el.dataset.cod || el.dataset.produtoid || 0);
                const nome = el.dataset.nome || el.querySelector('.prato-nome')?.innerText || '';
                const precoRaw = el.dataset.preco || el.dataset.price || el.querySelector('.prato-preco')?.innerText || '0';
                const preco = parseFloat(String(precoRaw).replace(/[^\d,.-]/g, '').replace(',', '.')) || 0;
                const img = el.dataset.img || el.querySelector('img')?.getAttribute('src') || '/img/placeholder.png';

                const payload = {
                    ProdutoId: produtoId,
                    Nome: nome,
                    Preco: preco,
                    Quantidade: 1,
                    Imagem: img
                };

                try {
                    const res = await fetch('/Carrinho/Adicionar', {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify(payload)
                    });

                    if (res.status === 401) {
                        alert('Você precisa estar logado para adicionar ao carrinho.');
                        window.location.href = '/Usuario/Login';
                        return;
                    }

                    await abrirEAposAdicionar();
                } catch (err) {
                    console.error(err);
                }
            });
        });
    }

    async function abrirEAposAdicionar() {
        await loadCart();
        abrirCarrinho();
    }

    document.addEventListener('DOMContentLoaded', function () {
        bindAddButtons();
    });

    window.abrirCarrinho = abrirCarrinho;
    window.fecharCarrinho = fecharCarrinho;
})();
