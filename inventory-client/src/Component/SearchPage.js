import React, { useState } from "react";
import { t } from "i18next"
import { useTranslation } from "react-i18next";

const SearchPage = () => {
  const {t} = useTranslation()
  const [searchQuery, setSearchQuery] = useState("");
  const [filter, setFilter] = useState("all");

  const handleSearchChange = (e) => {
    setSearchQuery(e.target.value);
  };

  const handleFilterChange = (e) => {
    setFilter(e.target.value);
  };

  const handleSearchSubmit = (e) => {
    e.preventDefault();
    // Here you can add code to fetch the search results using the searchQuery and filter state values
    console.log(`Searching for ${searchQuery} with filter ${filter}`);
  };

  return (
    <div className="search-page">
      <form onSubmit={handleSearchSubmit}>
        <input
          type="text"
          placeholder="Search for items..."
          value={searchQuery}
          onChange={handleSearchChange}
        />
        <select value={filter} onChange={handleFilterChange}>
          <option value="all">{t('All')}</option>
          <option value="available">{t('Available')}</option>
          <option value="unavailable">{t('Unavailable')}</option>
        </select>
        <button type="submit">{t('Search')}</button>
      </form>
    </div>
  );
};

export default SearchPage;