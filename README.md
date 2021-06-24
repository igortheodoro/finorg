# Finorg

![Demonstração do bot funcionando no Telegram](https://i.imgur.com/iFKJs2y.gif)

> Finorg é um bot para telegram que auxilia pessoas a organizarem melhor as finanças pessoais.

### ⚙️ Ajustes e melhorias

O projeto ainda está em desenvolvimento e as próximas atualizações serão voltadas nas seguintes tarefas:

- [ ] Melhorar o detalhamento dos relatórios mensais.
- [ ] Salvar as informações da API de FIIs e Stocks em cache.
- [ ] Salvar a moeda dos ganhos e gastos (real, dólar, bitcoin...).
- [ ] Adicionar dívidas e créditos com entidades (/divida 1 bitcoin para João da Silva).
- [ ] Adicionar avisos e dicas.
- [ ] Adicionar informações sobre os preços das moedas e ações brasileiras.
- [ ] Adicionar segmentos nos ganhos e gastos (-5 reais comidas)

## 📫 Contribuindo
Para contribuir com o projeto, siga estas etapas:

1. Bifurque este repositório.
2. Crie um branch: `git checkout -b <nome_branch>`.
3. Faça suas alterações e confirme-as: `git commit -m '<mensagem_commit>'`
4. Envie para o branch original.
5. Crie a solicitação de pull.

Como alternativa, consulte a documentação do GitHub em [como criar uma solicitação pull](https://help.github.com/en/github/collaborating-with-issues-and-pull-requests/creating-a-pull-request).

## Configurações necessárias
Para utilizar o projeto após clonar é preciso configurar o appsettings:
1. Na pasta Finorg criei um arquivo com o nome appsettings.json.
2. Cole o seguinte json nesse arquivo: 
```
{
  "TelegramKey": "SuaKey",
  "StockExchangeApi": {
    "Url": "https://mfinance.com.br/",
    "Fiis": "api/v1/fiis",
    "Stocks": "api/v1/stocks"
  },
  "ConnectionStrings": {
    "DataConnection": "StringDeConexão" }
}
```
3. Insira nesse arquivo sua key e sua string connection do banco de dados.

## 🤝 Criador

<table>
  <tr>
    <td align="center">
      <a href="#">
        <img src="https://avatars.githubusercontent.com/u/45567815?s=400&u=81a496ea70b6eb5fffa290e7bb594ffff93a6093&v=4" width="100px;" alt="Foto do Igor Theodoro no GitHub"/><br>
        <sub>
          <b>Igor Theodoro</b>
        </sub>
      </a>
    </td>
  </tr>
</table>
