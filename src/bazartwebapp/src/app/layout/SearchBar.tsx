import { observer } from "mobx-react-lite";
import React, { useState } from "react";
import { Header, Search, SearchResultProps } from "semantic-ui-react";
import { SearchItem } from "../models/Search/SearchItem";
import { Searching } from "../models/Search/Searching";
import { useStore } from "../stores/store";
import { router } from "../router/Routes";

export default observer(function SearchBar() {
    const { searchStore } = useStore();

    const [isLoading, setIsLoading] = useState(false);
    const [result, setResult] = useState<Searching>({
        items: {},
        value: ''
    });

    const handleResultSelect = (_e: React.MouseEvent<HTMLElement>, { result }: { result: SearchItem }) => {
        router.navigate(result.link);
        searchStore.setVisibleSearchBar(false);
    }

    const handleSearchChange = async (_e: React.MouseEvent<HTMLElement>, { value }: { value?: string }) => {
        if (value === '') {
            setResult({ value: value, items: {} });
        } else {
            setIsLoading(true);
            await searchStore.search(value!);
            setResult({ value: value || '', items: searchStore.results.items });
            setIsLoading(false);
        }
    }

    const categoryLayoutRenderer = ({ categoryContent, resultsContent }: { categoryContent: React.ReactNode, resultsContent: React.ReactNode }) => (
        <div style={{ maxHeight: '250px', overflowY: 'auto' }}>
            <div className='background-color-gold' style={{ position: 'sticky', top: '0' }}>
                <Header size='large' className='name' textAlign='center'>{categoryContent}</Header>
            </div>
            <div className='results background-color-creme'>
                {resultsContent}
            </div>
        </div>
    )

    const resultRender = ({ title, description, image }: SearchResultProps) => {
        return (
            <>
                <div className='image'>
                    <img src={image} />
                </div>
                <div className='content'>
                    <div className='title'>{title.length > 30 ? title.slice(0,30) + '(...)' : title}</div>
                    {description
                        ? <div className='description'>{description.length > 70 ? description.slice(0,70) + '(...)' : description}</div>
                        : <></>}
                </div>
            </>
        )
    }

    return (
        <>
            <div style={{ display: 'flex', justifyContent: 'center', margin: '5px' }} >
                <Search
                    results={result.items}
                    category
                    loading={isLoading}
                    onResultSelect={handleResultSelect}
                    onSearchChange={handleSearchChange}
                    placeholder='Search'
                    size='large'
                    value={result.value}
                    minCharacters={1}
                    categoryLayoutRenderer={categoryLayoutRenderer}
                    resultRenderer={resultRender}
                />
            </div>
        </>
    )
})