import React,{useState,useContext,useEffect} from 'react';
import { useCallback } from 'react';
const URL = "http://openlibrary.org/search.json?author=";
const recommendationContext = React.createContext();

const RecommendProvider = ({children})=>{
    const [searchTerm, setSearchTerm]= useState("James Patterson");
    const [books, setBooks] = useState([]);
    const [loading, setLoading] = useState(true);
    const [resultTitle, setResultTitle] = useState('');

    const fetchBooks = useCallback(async()=>{
        setLoading(true);
        try{
            const response = await fetch(`${URL}${searchTerm}`);
            const data = await response.json();
            const {docs} = data;
            //console.log("TheData: "+JSON.stringify(data));

            if(docs){
                const newBooks = docs.slice(0,20).map((bookSingle)=>{
                    const {key,author_name,cover_i, edition_count, 
                    first_publish_year, title}= bookSingle;

                    return{
                        id: key,
                        author: author_name,
                        cover_id: cover_i,
                        edition_count: edition_count,
                        first_publish_year: first_publish_year,
                        title: title

                    }

                });
                setBooks(newBooks);
                if(newBooks.length > 1){
                    setResultTitle('Recommendations');
                }
                /*else{
                    setResultTitle('No Search Result Found!');
                }*/
            }else{
                setBooks([]);
                setResultTitle("No Recommendations Found");
            }
            setLoading(false);
        }catch(error){
            console.log(error);
            setLoading(false);
        }
    },[searchTerm]);

    useEffect(()=>{
        fetchBooks();
    }, [searchTerm,fetchBooks]);

    return(
        <recommendationContext.Provider value = {{
            loading, books, setSearchTerm, resultTitle,setResultTitle,
        }}>
            {children}
        </recommendationContext.Provider>
    )

}

export const useGlobalRecommendContext = () => {
    return useContext(recommendationContext);
}

export {recommendationContext, RecommendProvider};