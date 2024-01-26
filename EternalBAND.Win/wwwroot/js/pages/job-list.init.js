var getChoices = document.querySelectorAll(".filter-input-box");
getChoices.forEach(element => new Choices(element,{
  
    shouldSort: false,
}));


// var singleCategories,singleLocation=new Choices("#choices-single-locationnn"),singleCategorie=document.getElementById("choices-single-categories");singleCategorie&&(singleCategories=new Choices("#choices-single-categories"));