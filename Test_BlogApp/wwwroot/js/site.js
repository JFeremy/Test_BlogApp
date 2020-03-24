const uri = 'Bloggers';
let bloggers = [];
let posts = [];
let favorite = [];

/*******************************************************************************************
        DISPLAY ITEMSS
*******************************************************************************************/
const displayFavoriteButton = '<div class="btn-addfavorite"><i class="far fa-star"></i> <em>Ajouter aux favoris</em></div>';

const displayBlogger = (editor) => {
    const { lastname, firstname, posts, id } = editor;
    const author = `<h4>${firstname} ${lastname}</h4>`;
    const nbPosts = `<p><em>${posts ? posts.length : 0} article${posts ? posts.length > 1 ? 's' : '' : ''}</em></p>`;
    const blogger = `<div class='blogger' id='author-${id}'>${author}${nbPosts}</div>`;
    $('#bloggers')
        .append(blogger);
};

const displayPost = (post) => {
    const { title, content, id } = post;
    const postTitle = `<h5>${title.toUpperCase()}</h5>`;
    const postContent = `<p>${content}</p>`;
    const isFavorite = favorite.find(item => item.id === id);
    const postElement = `<div class='post' id='post-${id}'>${postTitle}${postContent}${isFavorite ? '' : displayFavoriteButton}</div>`;
    $('#posts')
        .append(postElement);
};

const displayFavoritePost = (post) => {
    const { title, content, id, idBlogger } = post;
    const author = bloggers.find(blogger => blogger.id === idBlogger);
    const postTitle = `<h5>${title.toUpperCase()}</h5>`;
    const postAuthor = `<h6><em>${author.firstname} ${author.lastname}</em></h5>`;
    const postContent = `<p>${content}</p>`;
    const postFavorite = `<div class="btn-removefavorite"><i class="fas fa-star"></i> <em>Retirer des favoris</em></div>`;
    const postElement = `<div class='favoritepost' id='favoritepost-${id}'>${postTitle}${postAuthor}${postContent}${postFavorite}</div>`;
    $('#favorite')
        .append(postElement);
};

/*******************************************************************************************
        DISPLAY CATEGORIES
*******************************************************************************************/
const displayBloggers = () => {
    bloggers.forEach(editor => {
        displayBlogger(editor)
    })
};

const displayPosts = (author) => {
    const authorId = author ? author.id : bloggers[0].id;
    const blogger = bloggers.find(blogger => blogger.id === authorId);
    const authorName = `${blogger.firstname} ${blogger.lastname}`;
    $("#author").html(`Les articles de ${authorName}`);
    $(`#author-${authorId}`).toggleClass("active");
    posts = [...blogger.posts];
    posts.forEach(post => {
        displayPost(post);
    });
};

const displayFavorites = () => {
    favorite.forEach(post => {
        displayFavoritePost(post);
    })
};


/*******************************************************************************************
        CLICK
*******************************************************************************************/
const onAuthorClick = (author) => {
    $(".active").toggleClass("active");
    $('.post').remove();
    displayPosts(author);
}

const onAddFavoriteClick = (idPost) => {
    $(`#post-${idPost} .btn-addfavorite`).remove()
    $('.favoritepost').remove();
    displayFavorites();
}

const onRemoveFavoriteClick = (idPost) => {
    const idFavorite = favorite.findIndex(item => item.id === parseInt(idPost));
    favorite.splice(idFavorite, 1);
    $(`#post-${idPost}`).append(displayFavoriteButton);
    $('.favoritepost').remove();
    displayFavorites();
}

/*******************************************************************************************
        EVENTS
*******************************************************************************************/
const getBloggers = () => {
    fetch(uri)
        .then(response => response.json())
        .then(data => {
            bloggers = [...data];
            displayBloggers();
            displayPosts(null);
            displayFavorites();
        })
        .catch(error => console.error('Unable to get items.', error));
};

$(document).ready(() => {
    $(document).on("click", ".blogger", event => {
        const idAuthor = event.currentTarget.id.replace(/author-/g, '');
        const blogger = bloggers.find(blogger => blogger.id === parseInt(idAuthor));
        onAuthorClick(blogger);
    });
    $(document).on("click", ".btn-addfavorite", event => {
        const idPost = event.currentTarget.parentElement.id.replace(/post-/g, '');
        favorite.push(posts.find(post => post.id === parseInt(idPost)));
        onAddFavoriteClick(idPost);
    });
    $(document).on("click", ".btn-removefavorite", event => {
        const idPost = event.currentTarget.parentElement.id.replace(/favoritepost-/g, '');
        onRemoveFavoriteClick(idPost);
    });
});