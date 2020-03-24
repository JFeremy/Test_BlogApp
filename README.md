# Test_BlogApp
Application de blog - Test d'ASP NET


## JSON  
Utilisation du site https://www.json-generator.com/ pour la génération automatique.
``[
  '{{repeat(5, 7)}}',
  {
    id: '{{index()}}',
    firstname: '{{firstName()}}',
    lastname: '{{surname()}}',
    post: [
      '{{repeat(1,5)}}',
      {
        id: '{{index()}}',
        title: '{{lorem(5, "words")}}',
        content: '{{lorem(1, "paragraphs")}}'
      }
    ]
  }
]``